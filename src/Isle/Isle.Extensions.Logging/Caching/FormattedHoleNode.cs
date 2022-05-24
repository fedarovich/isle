namespace Isle.Extensions.Logging.Caching;

internal sealed class FormattedHoleNode : Node
{
    public FormattedHoleNode(Node parent, string name, int alignment, string? format) : base(parent)
    {
        Name = name;
        Format = format;
        Alignment = alignment;
    }

    public string Name { get; }
    
    public string? Format { get; }
    
    public int Alignment { get; }
}