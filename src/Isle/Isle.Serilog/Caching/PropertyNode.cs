using System.Runtime.CompilerServices;
using Serilog.Parsing;

namespace Isle.Serilog.Caching;

internal sealed class PropertyNode : Node
{
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    public PropertyNode(Node parent, string name, string rawName, string? format = null, int alignment = 0) 
        : base(parent, GetRawText(name, format, alignment))
    {
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

    public override PropertyToken Token { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Alignment GetAlignment(int alignment) => alignment <= 0
        ? new Alignment(AlignmentDirection.Left, -alignment)
        : new Alignment(AlignmentDirection.Right, alignment);

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