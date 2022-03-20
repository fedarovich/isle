#nullable enable
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Isle.Configuration;

namespace Isle.Extensions.Logging;


[InterpolatedStringHandler]
public ref partial struct TraceLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct DebugLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct InformationLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct WarningLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct ErrorLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct CriticalLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct LogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

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
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
        }
    }

    public bool IsEnabled => _originalFormatBuilder != null;

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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}


[InterpolatedStringHandler]
public ref partial struct ScopeLogInterpolatedStringHandler
{
    private const string OriginalFormatName = "{OriginalFormat}";
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private StringBuilder _originalFormatBuilder = null!;
    private FormattedLogValue[] _formattedLogValues = null!;
    private Segment[] _segments = null!;
    private int _valueIndex = 0;
    private int _segmentIndex = 0;

    public ScopeLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger    )
    {
            _originalFormatBuilder = StringBuilderCache.Acquire(Math.Max(literalLength + formattedCount * 16, StringBuilderCache.MaxBuilderSize));
            _formattedLogValues = new FormattedLogValue[formattedCount + 1];
            _segments = GC.AllocateUninitializedArray<Segment>(formattedCount * 2 + 1);
    }


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

    public void AppendFormatted(string? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, default, default), 
        alignment, 
        format);

    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => AppendFormatted(
        new NamedLogValue(value, name, IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(), default), 
        alignment, 
        format);

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
        _segments[_segmentIndex++] = new Segment(alignment);
        _formattedLogValues[_valueIndex++] = new (name, namedLogValue.Value, format);
    }

    internal FormattedLogValues GetFormattedLogValuesAndReset()
    {
        _formattedLogValues[_valueIndex] = new FormattedLogValue(OriginalFormatName, StringBuilderCache.GetStringAndRelease(_originalFormatBuilder));
        var result = new FormattedLogValues(_formattedLogValues, _segments, _segmentIndex);
        _formattedLogValues = null!;
        _originalFormatBuilder = null!;
        _segments = null!;
        return result;
    }
}

