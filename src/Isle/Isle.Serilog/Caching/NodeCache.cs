using System.Collections.Concurrent;
using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal sealed class NodeCache : Node
{
    public static readonly NodeCache Instance = new();

    private NodeCache()
    {
        Init(
            textNodes: new ConcurrentDictionary<string, TextNode>(),
            propertyNodes: new ConcurrentDictionary<string, PropertyNode>(),
            formattedPropertyNodes: new ConcurrentDictionary<FormatKey, PropertyNode>(),
            templateNode: new TemplateNode(this)
        );
    }

    static NodeCache()
    {
    }

    public override MessageTemplateToken GetToken() => null!;

    protected internal override void Reset()
    {
        Init(
            textNodes: new ConcurrentDictionary<string, TextNode>(),
            propertyNodes: new ConcurrentDictionary<string, PropertyNode>(),
            formattedPropertyNodes: new ConcurrentDictionary<FormatKey, PropertyNode>(),
            templateNode: GetTemplateNode()
        );
    }
}