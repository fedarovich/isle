using System.Globalization;
using System.Runtime.CompilerServices;

namespace Isle.Converters;

internal sealed class SerilogCompatibleValueNameConverter
{
    private const int MaxStackallocBufferSize = 256;
    
    private readonly bool _capitalizeFirstCharacter;

    internal SerilogCompatibleValueNameConverter(bool capitalizeFirstCharacter)
    {
        _capitalizeFirstCharacter = capitalizeFirstCharacter;
    }
    
    internal string Convert(string expression)
    {
        if (string.IsNullOrEmpty(expression))
            throw new ArgumentException("The expression cannot be null or empty string.", nameof(expression));

        int position = GetFirstPositionToRemove(expression);
        if (position >= 0)
            return Convert(expression, position, _capitalizeFirstCharacter);

        if (_capitalizeFirstCharacter && char.GetUnicodeCategory(expression[0]) == UnicodeCategory.LowercaseLetter)
            return CapitalizeFirstCharacter(expression);

        return expression;
    }

    private static int GetFirstPositionToRemove(string expression)
    {
        for (int pos = 0; pos < expression.Length; pos++)
        {
            char c = expression[pos];
            if (!(char.IsLetterOrDigit(c) || c == '_'))
                return pos;
        }

        return -1;
    }

    private static string CapitalizeFirstCharacter(string expression) =>
        string.Create(expression.Length, expression, (span, expr) =>
        {
            span[0] = char.ToUpperInvariant(expr[0]);
            expr.AsSpan(1).CopyTo(span.Slice(1));
        });

    [SkipLocalsInit]
    private static string Convert(in ReadOnlySpan<char> expression, int position, bool capitalizeFirstCharacter)
    {
        var vsb = expression.Length <= MaxStackallocBufferSize
            ? new ValueStringBuilder(stackalloc char[expression.Length])
            : new ValueStringBuilder(expression.Length);

        vsb.Append(expression.Slice(0, position));

        int start = position + 1;
        for (position = start; position < expression.Length; position++)
        {
            char c = expression[position];
            if (char.IsLetterOrDigit(c) || c == '_')
                continue;

            if (start < position)
            {
                vsb.Append(expression[start..position]);
            }

            start = position + 1;
        }

        if (start < expression.Length)
        {
            vsb.Append(expression.Slice(start));
        }

        if (vsb.Length == 0)
        {
            vsb.Dispose();
            throw new ArgumentException("The expression must contain at least one letter, digit or underscore character.", nameof(expression));
        }

        ref var firstChar = ref vsb[0];
        if (capitalizeFirstCharacter && char.GetUnicodeCategory(firstChar) == UnicodeCategory.LowercaseLetter)
        {
            firstChar = char.ToUpperInvariant(firstChar);
        }

        return vsb.ToString();
    }
}