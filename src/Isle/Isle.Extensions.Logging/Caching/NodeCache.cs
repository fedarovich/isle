namespace Isle.Extensions.Logging.Caching;

internal sealed class NodeCache : Node
{
    public static readonly NodeCache Instance = new NodeCache();
    
    private NodeCache()
    {
    }

    static NodeCache()
    {
    }

    internal new void Reset() => base.Reset();
}