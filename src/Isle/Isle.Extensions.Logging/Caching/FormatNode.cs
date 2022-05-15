namespace Isle.Extensions.Logging.Caching;

internal sealed class FormatNode : Node
{
    public FormatNode(Node parent, string name, string? format, int alignment) : base(parent)
    {
        Name = name;
        Format = format;
        Alignment = alignment;
    }

    public string Name { get; }
    
    public string? Format { get; }
    
    public int Alignment { get; }
}