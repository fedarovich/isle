namespace Isle.Extensions.Logging.Caching;

internal sealed class HoleNode : Node
{
    public HoleNode(Node parent, string name) : base(parent)
    {
        Name = name;
    }

    public string Name { get; }
}