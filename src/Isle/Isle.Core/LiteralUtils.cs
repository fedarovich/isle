using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Isle;

internal static class LiteralUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string EscapeLiteral(string literal)
    {
        var literalSpan = literal.AsSpan();
        int braceIndex = literalSpan.IndexOfAny('{', '}');
        return braceIndex >= 0 ? BuildEscapedString(braceIndex, literalSpan) : literal;
    }

    [SkipLocalsInit]
    private static string BuildEscapedString(int braceIndex, in ReadOnlySpan<char> strSpan)
    {
        int remainingLength = strSpan.Length - braceIndex;
        // Assume that the brace count does not exceed 12.5%.
        int builderInitialCapacity = braceIndex + remainingLength + Math.Max(remainingLength >> 3, 4);
        var builder = builderInitialCapacity <= 1024
            ? new ValueStringBuilder(stackalloc char[builderInitialCapacity])
            : new ValueStringBuilder(builderInitialCapacity);
        
        ReadOnlySpan<char> span = strSpan;
        do
        {
            int length = braceIndex + 1;
            var slice = span.Slice(0, length);
            builder.Append(slice);
            builder.Append(Unsafe.Add(ref MemoryMarshal.GetReference(span), braceIndex));
            span = span.Slice(length);
            braceIndex = span.IndexOfAny('{', '}');
        } while (braceIndex >= 0);

        builder.Append(span);

        return builder.ToString();
    }
}