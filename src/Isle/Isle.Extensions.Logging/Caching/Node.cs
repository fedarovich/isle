using System.Collections.Concurrent;

namespace Isle.Extensions.Logging.Caching;

internal abstract class Node
{
    private ConcurrentDictionary<string, LiteralNode>? _literalNodes;

    private ConcurrentDictionary<FormatKey, FormatNode>? _formatNodes;

    private TemplateNode? _templateNode;

    protected Node(Node? parent = null)
    {
        Parent = parent;
        Depth = parent != null ? (parent.Depth + 1) : 0;
    }

    public Node? Parent { get; }

    public int Depth { get; }

    private ConcurrentDictionary<string, LiteralNode> LiteralNodes => _literalNodes ??= new();

    private ConcurrentDictionary<FormatKey, FormatNode> FormatNodes => _formatNodes ??= new();

    public LiteralNode GetOrAddLiteralNode(string rawLiteral)
    {
        return LiteralNodes.GetOrAdd(rawLiteral, static (rl, parent) => new LiteralNode(parent, rl), this);
    }

    public FormatNode GetOrAddFormatNode(in NamedLogValue namedLogValue, int alignment, string? format)
    {
        var formatKey = new FormatKey(namedLogValue.Type, namedLogValue.Name, format, alignment, namedLogValue.HasExplicitName);
        return FormatNodes.GetOrAdd(formatKey, static (key, arg) => 
            new FormatNode(arg.parent, arg.namedLogValue, key.Format, key.Alignment), (parent: this, namedLogValue));
    }

    public TemplateNode GetTemplateNode()
    {
        if (_templateNode != null)
            return _templateNode;

        var templateNode = new TemplateNode(this);
        return Interlocked.CompareExchange(ref _templateNode, templateNode, null) ?? templateNode;
    }

    protected void Reset()
    {
        _literalNodes?.Clear();
        _formatNodes?.Clear();
        _templateNode = null;
    }
}