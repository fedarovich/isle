using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal sealed class TextNode : Node
{
    public TextNode(Node parent, string text) : base(parent, LiteralUtils.EscapeLiteral(text))
    {
        Token = new TextToken(text, Offset);
    }

    public override TextToken Token { get; }
}