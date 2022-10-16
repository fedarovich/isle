using System.Runtime.CompilerServices;
using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal sealed class PropertyNode : Node
{
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    public PropertyNode(Node parent, string name, string rawName, int alignment = 0, string? format = null) 
        : base(parent, GetRawText(name, format, alignment))
    {
        Name = name;

        var destructuring = Destructuring.Default;
        var propertyName = name;

        if (name.StartsWith(DestructureOperator))
        {
            destructuring = Destructuring.Destructure;
            propertyName = ReferenceEquals(name, rawName)
                ? name.Substring(1)
                : rawName;
        }
        else if (name.StartsWith(StringifyOperator))
        {
            destructuring = Destructuring.Stringify;
            propertyName = ReferenceEquals(name, rawName)
                ? name.Substring(1)
                : rawName;
        }

        Token = new PropertyToken(
            propertyName,
            RawText,
            format,
            GetAlignment(alignment),
            destructuring,
            Offset);
    }

    public string Name { get; }

    public FormatKey FormatKey => new (Name, Token.Format, GetAlignment(Token.Alignment ?? default));

    public PropertyToken Token { get; }

    public override MessageTemplateToken GetToken() => Token;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Alignment GetAlignment(int alignment) => alignment <= 0
        ? new Alignment(AlignmentDirection.Left, -alignment)
        : new Alignment(AlignmentDirection.Right, alignment);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetAlignment(Alignment alignment) =>
        alignment.Direction == AlignmentDirection.Right ? alignment.Width : -alignment.Width;    
        

    [SkipLocalsInit]
    private static string GetRawText(string name, string? format = null, int alignment = 0)
    {
        int bufferLength = name.Length + (format?.Length ?? 0) + 16;
        var vsb = bufferLength <= 256
            ? new ValueStringBuilder(stackalloc char[bufferLength])
            : new ValueStringBuilder(bufferLength);

        vsb.Append('{');
        vsb.Append(name);

        if (alignment != 0)
        {
            vsb.Append(',');
            vsb.AppendSpanFormattable(alignment);
        }

        if (!string.IsNullOrEmpty(format))
        {
            vsb.Append(':');
            vsb.Append(format);
        }

        vsb.Append('}');
        var rawText = vsb.ToString();
        return rawText;
    }
}