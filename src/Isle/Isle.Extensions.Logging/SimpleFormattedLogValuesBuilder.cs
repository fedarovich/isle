using System.Runtime.CompilerServices;
using System.Text;

namespace Isle.Extensions.Logging;

internal sealed class SimpleFormattedLogValuesBuilder : IFormattedLogValuesBuilder
{
    [ThreadStatic]
    private static SimpleFormattedLogValuesBuilder? _cachedInstance;

    private const int MaxStringBuilderCapacity = 512;

    private StringBuilder? _cachedStringBuilder;
    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValuesBase _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    private SimpleFormattedLogValuesBuilder()
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static SimpleFormattedLogValuesBuilder AcquireAndInitialize(int literalLength, int formattedCount, FormattedLogValuesBase formattedLogValues)
    {
        var instance = _cachedInstance ?? new SimpleFormattedLogValuesBuilder();
        _cachedInstance = null;
        instance.Initialize(literalLength, formattedCount, formattedLogValues);
        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize(int literalLength, int formattedCount, FormattedLogValuesBase formattedLogValues)
    {
        _originalFormatBuilder = AcquireStringBuilder();
        _formattedLogValues = formattedLogValues;
        _segments = new Segment[formattedCount * 2 + 1];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        StringBuilder AcquireStringBuilder()
        {
            var capacity = Math.Max(literalLength + formattedCount * 16, MaxStringBuilderCapacity);
            var cachedStringBuilder = _cachedStringBuilder;
            if (cachedStringBuilder == null || cachedStringBuilder.Capacity < capacity)
                return new StringBuilder(capacity);

            _cachedStringBuilder = null;
            return cachedStringBuilder;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public FormattedLogValuesBase BuildAndReset()
    {
        var result = _formattedLogValues;
        result.Values[_valueIndex] = new KeyValuePair<string, object?>(FormattedLogValuesBuilder.OriginalFormatName, GetStringAndRelease(_originalFormatBuilder));
        result.Count = _valueIndex + 1;
        result.SetSegments(_segments.AsMemory(0, _segmentIndex));
        
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        _literalList = null!;
        _segmentIndex = 0;
        _valueIndex = 0;

        _cachedInstance ??= this;

        return result;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string GetStringAndRelease(StringBuilder stringBuilder)
        {
            var str = stringBuilder.ToString();
            if (stringBuilder.Capacity <= MaxStringBuilderCapacity)
            {
                stringBuilder.Clear();
                _cachedStringBuilder = stringBuilder;
            }

            return str;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AppendLiteral(string str)
    {
        var start = _originalFormatBuilder.Length;
        _originalFormatBuilder.EscapeAndAppend(str.AsSpan());
        var end = _originalFormatBuilder.Length;
        var length = end - start;
        if (length > 0)
        {
            if (_segmentIndex > 0)
            {
                ref var prevSegment = ref _segments[_segmentIndex - 1];
                switch (prevSegment.Type)
                {
                    case Segment.SegmentType.Literal:
                    {
                        prevSegment = new Segment(prevSegment, str, _literalList ??= new List<string>());
                        return;
                    }
                    case Segment.SegmentType.LiteralList:
                    {
                        prevSegment = new Segment(prevSegment, str);
                        return;
                    }
                }
            }

            _segments[_segmentIndex++] = new Segment(str);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public  void AppendLiteralValue(in LiteralValue literalValue)
    {
        AppendLiteral(literalValue.Value!);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AppendFormatted(string name, object? value)
    {
        _originalFormatBuilder.Append('{');
        _originalFormatBuilder.Append(name);
        _originalFormatBuilder.Append('}');
        _segments[_segmentIndex++] = new Segment(null, 0);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AppendFormatted(string name, object? value, int alignment, string? format)
    {
        _originalFormatBuilder.Append('{');

        _originalFormatBuilder.Append(name);

        if (alignment != 0)
        {
            _originalFormatBuilder.Append(',');
            _originalFormatBuilder.Append(alignment);
        }

        if (!string.IsNullOrEmpty(format))
        {

            _originalFormatBuilder.Append(':');
            _originalFormatBuilder.Append(format);
        }

        _originalFormatBuilder.Append('}');
        _segments[_segmentIndex++] = new Segment(format, alignment);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }
}