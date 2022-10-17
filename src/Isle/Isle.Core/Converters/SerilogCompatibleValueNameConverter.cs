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

        int position = GetFirstPositionToRemove(expression, out var hasAt);
        if (position >= 0)
            return Convert(expression.AsSpan(), position, _capitalizeFirstCharacter, hasAt);

        return _capitalizeFirstCharacter 
            ? CapitalizeFirstCharacterConverter.Convert(expression) 
            : (expression != "@" ? expression : throw new ArgumentException("The expression cannot consist of '@' character only."));
    }

    private static int GetFirstPositionToRemove(string expression, out bool hasAt)
    {
        hasAt = false;
        for (int pos = 0; pos < expression.Length; pos++)
        {
            char c = expression[pos];
            if (!(char.IsLetterOrDigit(c) || c == '_'))
            {
                if (c == '@' && !hasAt)
                {
                    hasAt = true;
                    continue;
                }

                return pos;
            }
        }

        return -1;
    }

    [SkipLocalsInit]
    private static string Convert(in ReadOnlySpan<char> expression, int position, bool capitalizeFirstCharacter, bool hasAt)
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

            if (c == '@' && !hasAt)
            {
                hasAt = true;
                continue;
            }

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

        if (vsb.Length == (hasAt ? 1 : 0))
        {
            vsb.Dispose();
            throw new ArgumentException("The expression must contain at least one letter, digit or underscore character.", nameof(expression));
        }

        ref var firstChar = ref vsb[hasAt ? 1 : 0];
        if (capitalizeFirstCharacter && char.GetUnicodeCategory(firstChar) == UnicodeCategory.LowercaseLetter)
        {
            firstChar = char.ToUpperInvariant(firstChar);
        }

        return vsb.ToString();
    }
}