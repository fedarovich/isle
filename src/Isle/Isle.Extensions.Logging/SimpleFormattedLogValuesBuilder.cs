using System.Runtime.CompilerServices;
using System.Text;

namespace Isle.Extensions.Logging;

internal sealed class SimpleFormattedLogValuesBuilder : FormattedLogValuesBuilder
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

    public override bool IsCaching => false;

    private SimpleFormattedLogValuesBuilder()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FormattedLogValuesBuilder AcquireAndInitialize(int literalLength, int formattedCount)
    {
        var instance = _cachedInstance ?? new SimpleFormattedLogValuesBuilder();
        _cachedInstance = null;
        instance.Initialize(literalLength, formattedCount);
        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize(int literalLength, int formattedCount)
    {
        _originalFormatBuilder = AcquireStringBuilder();
        _formattedLogValues = FormattedLogValuesBase.Create(formattedCount);
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

    public override FormattedLogValuesBase BuildAndReset()
    {
        var result = _formattedLogValues;
        result.Values[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, GetStringAndRelease(_originalFormatBuilder));
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

    public override void AppendLiteral(string? str)
    {
        var start = _originalFormatBuilder.Length;
        _originalFormatBuilder.EscapeAndAppend(str);
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
                        prevSegment = new Segment(prevSegment, str!, _literalList ??= new List<string>());
                        return;
                    }
                    case Segment.SegmentType.LiteralList:
                    {
                        prevSegment = new Segment(prevSegment, str!);
                        return;
                    }
                }
            }

            _segments[_segmentIndex++] = new Segment(str!);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void AppendFormatted(string name, object? value, int alignment = 0, string? format = null)
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