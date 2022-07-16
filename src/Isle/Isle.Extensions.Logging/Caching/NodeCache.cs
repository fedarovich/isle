using System.Collections.Concurrent;

namespace Isle.Extensions.Logging.Caching;

internal sealed class NodeCache : Node
{
    public static readonly NodeCache Instance = new ();
    
    private NodeCache()
    {
        Init(
            literalNodes: new ConcurrentDictionary<string, LiteralNode>(),
            holeNodes: new ConcurrentDictionary<string, HoleNode>(),
            formattedHoleNodes: new ConcurrentDictionary<FormatKey, FormattedHoleNode>(),
            templateNode: new TemplateNode(this)
        );
    }

    static NodeCache()
    {
    }

    protected internal override void Reset()
    {
        Init(
            literalNodes: new ConcurrentDictionary<string, LiteralNode>(),
            holeNodes: new ConcurrentDictionary<string, HoleNode>(),
            formattedHoleNodes: new ConcurrentDictionary<FormatKey, FormattedHoleNode>(),
            templateNode: GetTemplateNode()
        );
    }
}