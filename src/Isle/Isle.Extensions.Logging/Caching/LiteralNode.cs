namespace Isle.Extensions.Logging.Caching;

internal sealed class LiteralNode : Node
{
    public LiteralNode(Node parent, string rawLiteral) : base(parent)
    {
        RawLiteral = rawLiteral;
        EscapedLiteral = rawLiteral.Replace("{", "{{").Replace("}", "}}");
    }

    public string RawLiteral { get; }

    public string EscapedLiteral { get; }
}