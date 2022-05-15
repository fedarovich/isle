using System.Collections.Concurrent;

namespace Isle.Extensions.Logging.Caching;

internal abstract class Node
{
    private ConcurrentDictionary<string, LiteralNode>? _literalNodes;
    
    private ConcurrentDictionary<string, HoleNode>? _holeNodes;

    private ConcurrentDictionary<FormatKey, FormattedHoleNode>? _formattedHoleNodes;

    private TemplateNode? _templateNode;

    protected Node(Node? parent = null)
    {
        Parent = parent;
        Depth = parent != null ? (parent.Depth + 1) : 0;
    }

    public Node? Parent { get; }

    public int Depth { get; }

    private ConcurrentDictionary<string, LiteralNode> LiteralNodes => _literalNodes ??= new();

    private ConcurrentDictionary<string, HoleNode> HoleNodes => _holeNodes ??= new();

    private ConcurrentDictionary<FormatKey, FormattedHoleNode> FormattedHoleNodes => _formattedHoleNodes ??= new();

    public LiteralNode GetOrAddLiteralNode(string rawLiteral)
    {
        return LiteralNodes.GetOrAdd(rawLiteral, static (rl, parent) => new LiteralNode(parent, rl), this);
    }

    public HoleNode GetOrAddHoleNode(string name)
    {
        return HoleNodes.GetOrAdd(name, static (n, parent) => new HoleNode(parent, n), this);
    }

    public FormattedHoleNode GetOrAddFormattedHoleNode(string name, int alignment, string? format)
    {
        var formatKey = new FormatKey(name, format, alignment);
        return FormattedHoleNodes.GetOrAdd(formatKey, static (key, arg) => 
            new FormattedHoleNode(arg.parent, key.Name, key.Format, key.Alignment), (parent: this, formatKey));
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
        _holeNodes?.Clear();
        _formattedHoleNodes?.Clear();
        _templateNode = null;
    }
}