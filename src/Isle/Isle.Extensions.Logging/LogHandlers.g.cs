#nullable enable
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Isle.Configuration;

namespace Isle.Extensions.Logging;


/// <summary>
/// Interpolated string handler for LogTrace() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct TraceLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="TraceLogInterpolatedStringHandler" />.
    /// </summary>
    public TraceLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogLevel.Trace);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for LogDebug() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct DebugLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="DebugLogInterpolatedStringHandler" />.
    /// </summary>
    public DebugLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogLevel.Debug);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for LogInformation() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct InformationLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="InformationLogInterpolatedStringHandler" />.
    /// </summary>
    public InformationLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogLevel.Information);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for LogWarning() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct WarningLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="WarningLogInterpolatedStringHandler" />.
    /// </summary>
    public WarningLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogLevel.Warning);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for LogError() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct ErrorLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="ErrorLogInterpolatedStringHandler" />.
    /// </summary>
    public ErrorLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogLevel.Error);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for LogCritical() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct CriticalLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="CriticalLogInterpolatedStringHandler" />.
    /// </summary>
    public CriticalLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogLevel.Critical);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for Log() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct LogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="LogInterpolatedStringHandler" />.
    /// </summary>
    public LogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        LogLevel logLevel,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(logLevel);
        if (isEnabled)
        {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _originalFormatBuilder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for BeginScope() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct ScopeLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private List<string> _literalList = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    /// <summary>
    /// Creates a new instance of <see cref="ScopeLogInterpolatedStringHandler" />.
    /// </summary>
    public ScopeLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger    )
    {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
            _segments = new Segment[formattedCount * 2 + 1];
    }


    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    public void AppendLiteral(string? str)
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

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="value" /> to the string log message.
    /// </summary>
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the string log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _originalFormatBuilder.Append('{');
        var name = namedLogValue.HasExplicitName ? namedLogValue.Name : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

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
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new KeyValuePair<string, object?>(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}

