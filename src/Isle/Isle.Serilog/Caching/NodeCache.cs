using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal class NodeCache : Node
{
    public static readonly NodeCache Instance = new();

    private NodeCache()
    {
    }

    static NodeCache()
    {
    }

    public override MessageTemplateToken Token => null!;
}