namespace Isle.Extensions.Logging.Caching;

internal sealed class NodeCache : Node
{
    public static readonly NodeCache Instance = new ();
    
    private NodeCache()
    {
    }

    static NodeCache()
    {
    }
}