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

    private ConcurrentDictionary<string, LiteralNode> LiteralNodes => 
        LazyInitializer.EnsureInitialized(ref _literalNodes, static () => new ());

    private ConcurrentDictionary<string, HoleNode> HoleNodes => 
        LazyInitializer.EnsureInitialized(ref _holeNodes, static () => new());

    private ConcurrentDictionary<FormatKey, FormattedHoleNode> FormattedHoleNodes => 
        LazyInitializer.EnsureInitialized(ref _formattedHoleNodes, static () => new());

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
            new FormattedHoleNode(arg.parent, key.Name, key.Alignment, key.Format), (parent: this, formatKey));
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
        Volatile.Read(ref _literalNodes)?.Clear();
        Volatile.Read(ref _holeNodes)?.Clear();
        Volatile.Read(ref _formattedHoleNodes)?.Clear();
        Volatile.Write(ref _templateNode, null);
    }
}