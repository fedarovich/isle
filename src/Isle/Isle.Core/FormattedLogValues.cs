using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Isle;

internal readonly record struct FormattedLogValue(string Name, object? Value, string? Format = null)
{
    public static implicit operator KeyValuePair<string, object?>(in FormattedLogValue formattedLogValue) =>
        new(formattedLogValue.Name, formattedLogValue.Value);
}

internal readonly struct FormattedLogValues : IReadOnlyList<KeyValuePair<string, object?>>
{
    private const string NullValue = "(null)";

    private readonly FormattedLogValue[] _values;
    private readonly Segment[] _segments;
    private readonly int _segmentCount;

    public FormattedLogValues(FormattedLogValue[] values, Segment[] segments, int segmentCount)
    {
        _values = values;
        _segments = segments;
        _segmentCount = segmentCount;
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        foreach (var formattedLogValue in _values)
        {
            yield return formattedLogValue;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _values.Length;

    public KeyValuePair<string, object?> this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(nameof(index));

            return _values[index];
        }
    }

    [SkipLocalsInit]
    public override string ToString()
    {
        int index = 0;

        var handler = new DefaultInterpolatedStringHandler(0, _segmentCount, CultureInfo.InvariantCulture, stackalloc char[256]);

        // ReSharper disable once UseIndexFromEndExpression
        var originalFormatSpan = ((string) _values[_values.Length - 1].Value!).AsSpan();
        for (var i = 0; i < _segments.Length && i < _segmentCount; i++)
        {
            var segment = _segments[i];
            if (segment.IsLiteral)
            {
                handler.AppendFormatted(originalFormatSpan.Slice(segment.Start, segment.Length));
            }
            else
            {
                var formattedValue = _values[index++];
                handler.AppendFormatted(FormatArgument(formattedValue.Value), segment.Alignment, formattedValue.Format);
            }
        }

        return handler.ToStringAndClear();
    }

    private static object FormatArgument(object? value)
    {
        if (value == null)
        {
            return NullValue;
        }

        // since 'string' implements IEnumerable, special case it
        if (value is string)
        {
            return value;
        }

        // if the value implements IEnumerable, build a comma separated string.
        if (value is IEnumerable enumerable)
        {
            var vsb = new StringBuilder(256);
            bool first = true;
            foreach (object? e in enumerable)
            {
                if (!first)
                {
                    vsb.Append(", ");
                }

                vsb.Append(e != null ? e.ToString() : NullValue);
                first = false;
            }
            return vsb.ToString();
        }

        return value;
    }
}