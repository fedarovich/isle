using System.Runtime.CompilerServices;
using System.Text;

namespace Isle.Extensions.Logging;

internal static class StringBuilderExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    internal static void EscapeAndAppend(this StringBuilder stringBuilder, in ReadOnlySpan<char> strSpan)
    {
        int start = 0;
        for (int end = 0; end < strSpan.Length; end++)
        {
            var c = strSpan[end];
            if (c is '{' or '}')
            {
                var length = end - start;
                if (length > 0)
                {
                    stringBuilder.Append(strSpan.Slice(start, length));
                }

                stringBuilder.Append(c);
                stringBuilder.Append(c);
                start = end + 1;
            }
        }

        if (start < strSpan.Length)
        {
            stringBuilder.Append(strSpan.Slice(start));
        }
    }
}