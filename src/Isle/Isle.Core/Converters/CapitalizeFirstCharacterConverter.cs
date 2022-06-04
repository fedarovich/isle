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
    private static string CapitalizeFirstCharacter(string expression) =>
        string.Create(expression.Length, expression, static (span, expr) =>
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
}