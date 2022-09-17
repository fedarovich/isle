using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Isle.Extensions.Logging;

internal abstract class FormattedLogValuesBase : IReadOnlyList<KeyValuePair<string, object?>>
{
    private const string NullValue = "(null)";

    private ReadOnlyMemory<Segment> _segments;

    private readonly int _maxCount;

    private int _count;

    protected FormattedLogValuesBase(int maxCount)
    {
        _count = _maxCount = maxCount;
    }

    internal abstract Span<KeyValuePair<string, object?>> Values { get; }

    internal void SetSegments(in ReadOnlyMemory<Segment> segments)
    {
        _segments = segments;
    }

    internal static FormattedLogValuesBase Create(int formattedCount) =>
        formattedCount switch
        {
            0 => new FormattedLogValues0(),
            1 => new FormattedLogValues1(),
            2 => new FormattedLogValues2(),
            3 => new FormattedLogValues3(),
            4 => new FormattedLogValues4(),
            5 => new FormattedLogValues5(),
            6 => new FormattedLogValues6(),
            7 => new FormattedLogValues7(),
            _ => new FormattedLogValues(formattedCount)
        };
   

    public abstract IEnumerator<KeyValuePair<string, object?>> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count
    {
        get => _count;
        internal set
        {
            if (value < 0 || value > _maxCount)
                throw new ArgumentOutOfRangeException(nameof(value));
            _count = value;
        }
    }

    public KeyValuePair<string, object?> this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException(nameof(index));

            return Values[index];
        }
    }

    [SkipLocalsInit]
    public override string ToString()
    {
        int index = 0;

        var handler = new DefaultInterpolatedStringHandler(0, _segments.Length, CultureInfo.InvariantCulture, stackalloc char[512]);

        foreach (var segment in _segments.Span)
        {
            if (segment.IsLiteral)
            {
                handler.AppendFormatted(segment.String);
            }
            else
            {
                var formattedValue = Values[index++];
                handler.AppendFormatted(FormatArgument(formattedValue.Value), segment.Alignment, segment.String);
            }
        }

        return handler.ToStringAndClear();
    }

    [SkipLocalsInit]
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
            var vsb = new ValueStringBuilder(stackalloc char[256]);
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