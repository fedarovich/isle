using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Isle.Extensions.Logging;

internal sealed class SimpleFormattedLogValuesBuilder : FormattedLogValuesBuilder
{
    [ThreadStatic]
    private static SimpleFormattedLogValuesBuilder? _cachedInstance;

    private FormattedLogValuesBase _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;
    private int _estimatedLength = 0;

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
        _estimatedLength = literalLength;
        _formattedLogValues = FormattedLogValuesBase.Create(formattedCount);
        _segments = new Segment[formattedCount * 2 + 4];
    }

    [SkipLocalsInit]
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override FormattedLogValuesBase BuildAndReset()
    {
        var result = _formattedLogValues;

        var estimatedLength = _estimatedLength + 16;
        var vsb = estimatedLength <= 512
            ? new ValueStringBuilder(stackalloc char[estimatedLength])
            : new ValueStringBuilder(estimatedLength);

        ref var value = ref MemoryMarshal.GetReference(result.Values);
        foreach (ref readonly var segment in _segments.AsSpan(0, _segmentIndex))
        {
            if (segment.IsLiteral)
            {
                vsb.EscapeAndAppend(segment.String);
            }
            else
            {
                vsb.Append('{');
                vsb.Append(value.Key);
                if (segment.Alignment != 0)
                {
                    vsb.Append(',');
                    vsb.AppendSpanFormattable(segment.Alignment);
                }

                if (!string.IsNullOrEmpty(segment.String))
                {
                    vsb.Append(':');
                    vsb.Append(segment.String);
                }

                vsb.Append('}');
                value = ref Unsafe.Add(ref value, 1);
            }
        }

        result.Values[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, vsb.ToString());
        result.Count = _valueIndex + 1;
        result.SetSegments(_segments.AsMemory(0, _segmentIndex));
        
        _formattedLogValues = null!;
        _segments = null!;
        _segmentIndex = 0;
        _valueIndex = 0;

        _cachedInstance ??= this;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void AppendLiteral(string? str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            EnsureSegmentsCapacity();
            _segments[_segmentIndex++] = new Segment(str);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public override void AppendFormatted(string name, object? value, int alignment = 0, string? format = null)
    {
        EnsureSegmentsCapacity();
        _segments[_segmentIndex++] = new Segment(format, alignment);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
        _estimatedLength += name.Length + (format?.Length ?? 0) + 8;
    }

    private void EnsureSegmentsCapacity()
    {
        if (_segmentIndex >= _segments.Length)
            Array.Resize(ref _segments, _segments.Length << 1);
    }
}