using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal abstract class Node
{
    // The next three fields can contain either a single child node or
    // a ConcurrentDictionary of child nodes if there are more than one

    private object? _textNodes;

    private object? _propertyNodes;

    private object? _formattedPropertyNodes;

    private TemplateNode? _templateNode;
    
    protected Node(Node? parent = null, string rawText = "")
    {
        Parent = parent;
        RawText = rawText;
        Depth = parent != null ? (parent.Depth + 1) : 0;
        Offset = parent != null ? parent.Offset + parent.RawText.Length : 0;
    }

    public Node? Parent { get; }

    public string RawText { get; }

    public int Depth { get; }

    public int Offset { get; }

    public abstract MessageTemplateToken GetToken();

    [MethodImpl(MethodImplOptions.NoInlining)]
    public TextNode GetOrAddTextNode(string rawLiteral)
    {
        var nextNode = Volatile.Read(ref _textNodes);
        TextNode? newNode = null;

        // If nextNode is null, we create a new Node and try to store it into the field
        if (nextNode == null)
        {
            newNode = new TextNode(this, rawLiteral);
            nextNode = Interlocked.CompareExchange(ref _textNodes, newNode, null);
            // If no other thread has changed the field, just return the new node
            if (nextNode == null)
                return newNode;
        }

        // Another thread has updated the field, so let's check whether it contains the node with the same key
        if (nextNode is TextNode textNode)
        {
            var nextNodeKey = textNode.Token.Text;
            // If the key is the same, just return the stored node
            if (nextNodeKey == rawLiteral) 
                return textNode;

            // Try to replace the field value with a ConcurrentDictionary containing the stored node.
            // If the update failed, it means that some other thread has already done the same,
            // so we need to do nothing at this time.
            Interlocked.CompareExchange(
                ref _textNodes, 
                new ConcurrentDictionary<string, TextNode> { [nextNodeKey] = textNode }, 
                textNode);
        }

        // At this point the field is guaranteed to have a ConcurrentDictionary,
        // so we can just call GetOrAdd to retrieve the node.
        var children = (Volatile.Read(ref _textNodes) as ConcurrentDictionary<string, TextNode>)!;
        return newNode != null
            ? children.GetOrAdd(rawLiteral, newNode)
            : children.GetOrAdd(rawLiteral, static (rl, parent) => new TextNode(parent, rl), this);
    }


    [MethodImpl(MethodImplOptions.NoInlining)]
    public PropertyNode GetOrAddPropertyNode(string name, string rawName)
    {
        var nextNode = Volatile.Read(ref _propertyNodes);
        PropertyNode? newNode = null;

        // If nextNode is null, we create a new Node and try to store it into the field
        if (nextNode == null)
        {
            newNode = new PropertyNode(this, name, rawName);
            nextNode = Interlocked.CompareExchange(ref _propertyNodes, newNode, null);
            if (nextNode == null)
                return newNode;
        }

        // Another thread has updated the field, so let's check whether it contains the node with the same key
        if (nextNode is PropertyNode propertyNode)
        {
            var nextNodeKey = propertyNode.Name;
            // If the key is the same, just return the stored node
            if (nextNodeKey == name)
                return propertyNode;

            // Try to replace the field value with a ConcurrentDictionary containing the stored node.
            // If the update failed, it means that some other thread has already done the same,
            // so we need to do nothing at this time.
            Interlocked.CompareExchange(
                ref _propertyNodes,
                new ConcurrentDictionary<string, PropertyNode> { [nextNodeKey] = propertyNode },
                propertyNode);
        }

        // At this point the field is guaranteed to have a ConcurrentDictionary,
        // so we can just call GetOrAdd to retrieve the node.
        var children = (Volatile.Read(ref _propertyNodes) as ConcurrentDictionary<string, PropertyNode>)!;
        return newNode != null
            ? children.GetOrAdd(name, newNode)
            : children.GetOrAdd(name, static (name, arg) => new PropertyNode(arg.parent, name, arg.rawName), (parent: this, rawName));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public PropertyNode GetOrAddPropertyNode(string name, string rawName, int alignment, string? format)
    {
        var nextNode = Volatile.Read(ref _formattedPropertyNodes);
        PropertyNode? newNode = null;

        // If nextNode is null, we create a new Node and try to store it into the field
        if (nextNode == null)
        {
            newNode = new PropertyNode(this, name, rawName, alignment, format);
            nextNode = Interlocked.CompareExchange(ref _formattedPropertyNodes, newNode, null);
            if (nextNode == null)
                return newNode;
        }

        // Another thread has updated the field, so let's check whether it contains the node with the same key
        var formatKey = new FormatKey(name, format, alignment);
        if (nextNode is PropertyNode propertyNode)
        {
            var nextNodeKey = propertyNode.FormatKey;
            // If the key is the same, just return the stored node
            if (nextNodeKey == formatKey)
                return propertyNode;

            // Try to replace the field value with a ConcurrentDictionary containing the stored node.
            // If the update failed, it means that some other thread has already done the same,
            // so we need to do nothing at this time.
            Interlocked.CompareExchange(
                ref _formattedPropertyNodes,
                new ConcurrentDictionary<FormatKey, PropertyNode> { [nextNodeKey] = propertyNode },
                propertyNode);
        }

        // At this point the field is guaranteed to have a ConcurrentDictionary,
        // so we can just call GetOrAdd to retrieve the node.
        var children = (Volatile.Read(ref _formattedPropertyNodes) as ConcurrentDictionary<FormatKey, PropertyNode>)!;
        return newNode != null
            ? children.GetOrAdd(formatKey, newNode)
            : children.GetOrAdd(
                formatKey, 
                static (formatKey, arg) => new PropertyNode(arg.parent, formatKey.Name, arg.rawName, formatKey.Alignment, formatKey.Format), (parent: this, rawName));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TextNode CreateNotCachedTextNode(string rawLiteral)
    {
        return new TextNode(this, rawLiteral);
    }

    public TemplateNode GetTemplateNode()
    {
        return Volatile.Read(ref _templateNode) ?? InitializeTemplateNode();

        [MethodImpl(MethodImplOptions.NoInlining)]
        TemplateNode InitializeTemplateNode()
        {
            var templateNode = new TemplateNode(this);
            Interlocked.CompareExchange(ref _templateNode, templateNode, null);
            return _templateNode;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void Init(
        object? textNodes = null, 
        object? propertyNodes = null, 
        object? formattedPropertyNodes = null, 
        TemplateNode? templateNode = null)
    {
        Volatile.Write(ref _textNodes, textNodes);
        Volatile.Write(ref _propertyNodes, propertyNodes);
        Volatile.Write(ref _formattedPropertyNodes, formattedPropertyNodes);
        Volatile.Write(ref _templateNode, templateNode);
    }

    protected internal virtual void Reset() => Init();
}