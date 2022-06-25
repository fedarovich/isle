using System.Collections.Concurrent;
using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal abstract class Node
{
    private ConcurrentDictionary<string, TextNode>? _textNodes;

    private ConcurrentDictionary<string, PropertyNode>? _propertyNodes;

    private ConcurrentDictionary<FormatKey, PropertyNode>? _formattedPropertyNodes;

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

    public abstract MessageTemplateToken Token { get; }

    private ConcurrentDictionary<string, TextNode> TextNodes =>
        LazyInitializer.EnsureInitialized(ref _textNodes, static () => new());

    private ConcurrentDictionary<string, PropertyNode> PropertyNodes =>
        LazyInitializer.EnsureInitialized(ref _propertyNodes, static () => new());

    private ConcurrentDictionary<FormatKey, PropertyNode> FormattedPropertyNodes =>
        LazyInitializer.EnsureInitialized(ref _formattedPropertyNodes, static () => new());

    public TextNode GetOrAddTextNode(string rawLiteral)
    {
        return TextNodes.GetOrAdd(rawLiteral, static (rl, parent) => new TextNode(parent, rl), this);
    }

    public PropertyNode GetOrAddPropertyNode(string name)
    {
        return PropertyNodes.GetOrAdd(name, static (n, parent) => new PropertyNode(parent, n), this);
    }

    public PropertyNode GetOrAddPropertyNode(string name, int alignment, string? format)
    {
        var formatKey = new FormatKey(name, format, alignment);
        return FormattedPropertyNodes.GetOrAdd(formatKey, static (key, arg) =>
            new PropertyNode(arg.parent, key.Name, key.Format, key.Alignment), (parent: this, formatKey));
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

    protected internal void Reset()
    {
        Volatile.Read(ref _textNodes)?.Clear();
        Volatile.Read(ref _propertyNodes)?.Clear();
        Volatile.Read(ref _formattedPropertyNodes)?.Clear();
        Volatile.Write(ref _templateNode, null!);
    }
}