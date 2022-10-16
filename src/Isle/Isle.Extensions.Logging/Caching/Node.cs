using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Isle.Extensions.Logging.Caching;

internal abstract class Node
{
    // The next three fields can contain either a single child node or
    // a ConcurrentDictionary of child nodes if there are more than one

    private object? _literalNodes;
    
    private object? _holeNodes;

    private object? _formattedHoleNodes;

    private TemplateNode? _templateNode;

    protected Node(Node? parent = null)
    {
        Parent = parent;
        Depth = parent != null ? (parent.Depth + 1) : 0;
    }

    public Node? Parent { get; }

    public int Depth { get; }

#if NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    public LiteralNode GetOrAddLiteralNode(string rawLiteral)
    {
        var nextNode = Volatile.Read(ref _literalNodes);
        LiteralNode? newNode = null;

        // If nextNode is null, we create a new Node and try to store it into the field
        if (nextNode == null)
        {
            newNode = new LiteralNode(this, rawLiteral);
            nextNode = Interlocked.CompareExchange(ref _literalNodes, newNode, null);
            // If no other thread has changed the field, just return the new node
            if (nextNode == null)
                return newNode;
        }

        // Another thread has updated the field, so let's check whether it contains the node with the same key
        if (nextNode is LiteralNode literalNode)
        {
            var nextNodeKey = literalNode.RawLiteral;
            // If the key is the same, just return the stored node
            if (nextNodeKey == rawLiteral)
                return literalNode;

            // Try to replace the field value with a ConcurrentDictionary containing the stored node.
            // If the update failed, it means that some other thread has already done the same,
            // so we need to do nothing at this time.
            Interlocked.CompareExchange(
                ref _literalNodes,
                new ConcurrentDictionary<string, LiteralNode> { [nextNodeKey] = literalNode },
                literalNode);
        }

        // At this point the field is guaranteed to have a ConcurrentDictionary,
        // so we can just call GetOrAdd to retrieve the node.
        var children = (Volatile.Read(ref _literalNodes) as ConcurrentDictionary<string, LiteralNode>)!;
        return newNode != null
            ? children.GetOrAdd(rawLiteral, newNode)
            : children.GetOrAdd(rawLiteral, static (rl, parent) => new LiteralNode(parent, rl), this);
    }

#if NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    public HoleNode GetOrAddHoleNode(string name)
    {
        var nextNode = Volatile.Read(ref _holeNodes);
        HoleNode? newNode = null;

        // If nextNode is null, we create a new Node and try to store it into the field
        if (nextNode == null)
        {
            newNode = new HoleNode(this, name);
            nextNode = Interlocked.CompareExchange(ref _holeNodes, newNode, null);
            if (nextNode == null)
                return newNode;
        }

        // Another thread has updated the field, so let's check whether it contains the node with the same key
        if (nextNode is HoleNode holeNode)
        {
            var nextNodeKey = holeNode.Name;
            // If the key is the same, just return the stored node
            if (nextNodeKey == name)
                return holeNode;

            // Try to replace the field value with a ConcurrentDictionary containing the stored node.
            // If the update failed, it means that some other thread has already done the same,
            // so we need to do nothing at this time.
            Interlocked.CompareExchange(
                ref _holeNodes,
                new ConcurrentDictionary<string, HoleNode> { [nextNodeKey] = holeNode },
                holeNode);
        }

        // At this point the field is guaranteed to have a ConcurrentDictionary,
        // so we can just call GetOrAdd to retrieve the node.
        var children = (Volatile.Read(ref _holeNodes) as ConcurrentDictionary<string, HoleNode>)!;
        return newNode != null
            ? children.GetOrAdd(name, newNode)
            : children.GetOrAdd(name, static (name, parent) => new HoleNode(parent, name), this);
    }

#if NETCOREAPP
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
    public FormattedHoleNode GetOrAddFormattedHoleNode(string name, int alignment, string? format)
    {
        var nextNode = Volatile.Read(ref _formattedHoleNodes);
        FormattedHoleNode? newNode = null;

        // If nextNode is null, we create a new Node and try to store it into the field
        if (nextNode == null)
        {
            newNode = new FormattedHoleNode(this, name, alignment, format);
            nextNode = Interlocked.CompareExchange(ref _formattedHoleNodes, newNode, null);
            if (nextNode == null)
                return newNode;
        }

        // Another thread has updated the field, so let's check whether it contains the node with the same key
        var formatKey = new FormatKey(name, format, alignment);
        if (nextNode is FormattedHoleNode formattedHoleNode)
        {
            var nextNodeKey = formattedHoleNode.FormatKey;
            // If the key is the same, just return the stored node
            if (nextNodeKey == formatKey)
                return formattedHoleNode;

            // Try to replace the field value with a ConcurrentDictionary containing the stored node.
            // If the update failed, it means that some other thread has already done the same,
            // so we need to do nothing at this time.
            Interlocked.CompareExchange(
                ref _formattedHoleNodes,
                new ConcurrentDictionary<FormatKey, FormattedHoleNode> { [nextNodeKey] = formattedHoleNode },
                formattedHoleNode);
        }

        // At this point the field is guaranteed to have a ConcurrentDictionary,
        // so we can just call GetOrAdd to retrieve the node.
        var children = (Volatile.Read(ref _formattedHoleNodes) as ConcurrentDictionary<FormatKey, FormattedHoleNode>)!;
        return newNode != null
            ? children.GetOrAdd(formatKey, newNode)
            : children.GetOrAdd(
                formatKey,
                static (formatKey, parent) => new FormattedHoleNode(parent, formatKey.Name, formatKey.Alignment, formatKey.Format), this);
    }

    public LiteralNode CreateNotCachedLiteralNode(string rawLiteral)
    {
        return new LiteralNode(this, rawLiteral);
    }

    public TemplateNode GetTemplateNode()
    {
        return Volatile.Read(ref _templateNode) ?? InitializeTemplateNode();

        TemplateNode InitializeTemplateNode()
        {
            var templateNode = new TemplateNode(this);
            Interlocked.CompareExchange(ref _templateNode, templateNode, null);
            return _templateNode;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void Init(
        object? literalNodes = null,
        object? holeNodes = null,
        object? formattedHoleNodes = null,
        TemplateNode? templateNode = null)
    {
        Volatile.Write(ref _literalNodes, literalNodes);
        Volatile.Write(ref _holeNodes, holeNodes);
        Volatile.Write(ref _formattedHoleNodes, formattedHoleNodes);
        Volatile.Write(ref _templateNode, templateNode);
    }

    protected internal virtual void Reset() => Init();
}