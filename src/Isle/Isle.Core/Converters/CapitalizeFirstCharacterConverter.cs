using System.Buffers;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Isle.Converters;

internal static class CapitalizeFirstCharacterConverter
{
    internal static string Convert(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            throw new ArgumentException("The expression cannot be null or empty string.");

        int firstCharIndex = expression[0] == '@' ? 1 : 0;
        if (firstCharIndex == 1 && expression.Length < 2)
            throw new ArgumentException("The expression cannot consist of '@' character only.");

        return char.GetUnicodeCategory(expression[firstCharIndex]) == UnicodeCategory.LowercaseLetter 
            ? CapitalizeFirstCharacter(expression) 
            : expression;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SkipLocalsInit]
    private static string CapitalizeFirstCharacter(string expression)
    {
#if NETCOREAPP || NETSTANDARD2_1
        return string.Create(expression.Length, expression, static (span, expr) =>
        {
            int index = 0;
            if (expr[0] == '@')
            {
                span[0] = '@';
                index++;
            }

            span[index] = char.ToUpperInvariant(expr[index]);
            index++;
            expr.AsSpan(index).CopyTo(span.Slice(index));
        });
#else
        char[]? array = null;
        try
        {
            Span<char> span = expression.Length <= 512
                ? stackalloc char[expression.Length]
                : (array = ArrayPool<char>.Shared.Rent(expression.Length));

            int index = 0;
            if (expression[0] == '@')
            {
                span[0] = '@';
                index++;
            }

            span[index] = char.ToUpperInvariant(expression[index]);
            index++;
            expression.AsSpan(index).CopyTo(span.Slice(index));
            unsafe
            {
                fixed (char* pChar = span)
                {
                    return new string(pChar, 0, expression.Length);
                }
            }
        }
        finally
        {
            if (array != null)
                ArrayPool<char>.Shared.Return(array);
        }
#endif
    }
}