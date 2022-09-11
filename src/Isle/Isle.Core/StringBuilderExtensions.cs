using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Isle;

internal static class StringBuilderExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    internal static void EscapeAndAppend(this StringBuilder stringBuilder, in ReadOnlySpan<char> strSpan)
    {
        ReadOnlySpan<char> span = strSpan;
        while (true)
        {
            int index = span.IndexOfAny('{', '}');
            if (index < 0)
                break;

            int length = index + 1;
            var slice = span.Slice(0, length);
            stringBuilder.Append(slice);
            ref char c = ref Unsafe.Add(ref MemoryMarshal.GetReference(span), index);
            stringBuilder.Append(c);
            span = span.Slice(length);
        }

        stringBuilder.Append(span);
    }
}