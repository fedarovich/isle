using System.Runtime.CompilerServices;
using System.Text;
using Isle.Configuration;

namespace Isle;
#if DEBUG
[InterpolatedStringHandler]
internal ref struct PrototypeLogHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";

    private readonly StringBuilder _originalFormatBuilder;
    private readonly FormattedLogValue[] _formattedLogValues;
    private readonly Segment[] _segments;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    public PrototypeLogHandler(int literalLength, int formattedCount, out bool isEnabled)
    {
        _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
        _formattedLogValues = new FormattedLogValue[formattedCount + 1];
        _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        isEnabled = true;
    }

    public bool IsEnabled => _segments != null;

    public void AppendLiteral(string? str)
    {
        var start = _originalFormatBuilder.Length;
        _originalFormatBuilder.EscapeAndAppend(str);
        var end = _originalFormatBuilder.Length;
        var length = end - start;
        if (length > 0)
        {
            if (_segmentIndex > 0 && _segments[_segmentIndex - 1].IsLiteral)
            {
                Segment.Grow(ref _segments[_segmentIndex - 1], length);
            }
            else
            {
                _segments[_segmentIndex++] = new Segment(start, length);
            }
        }
    }

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") 
        => AppendFormatted(new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), alignment, format);

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        if (!namedLogValue.Name.StartsWith('@') && !namedLogValue.Name.StartsWith('$'))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    _originalFormatBuilder.Append('@');
                    break;
                case ValueRepresentation.Stringify:
                    _originalFormatBuilder.Append('$');
                    break;
            }
        }

        _originalFormatBuilder.Append(namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name));

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (namedLogValue.Name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetSegmentedLogValues()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        return new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
    }
}
#endif