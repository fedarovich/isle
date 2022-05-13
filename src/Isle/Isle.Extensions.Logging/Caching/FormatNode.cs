namespace Isle.Extensions.Logging.Caching;

internal sealed class FormatNode : Node
{
    public FormatNode(Node parent, in NamedLogValue name, string? format, int alignment) : base(parent)
    {
        Name = FormattedLogValuesBuilder.TransformName(name);
        Format = format;
        Alignment = alignment;
    }

    public string Name { get; }
    
    public string? Format { get; }
    
    public int Alignment { get; }
}