#nullable enable
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

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
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
        }
    }

    /// <summary>
    /// Gets the value indicating whether the handler is enabled.
    /// </summary>
    public bool IsEnabled => _builder != null;

    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
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
    private FormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="ScopeLogInterpolatedStringHandler" />.
    /// </summary>
    public ScopeLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger    )
    {
            _builder = FormattedLogValuesBuilder.Acquire(literalLength, formattedCount);
    }


    /// <summary>
    /// Appends a string literal to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string? str)
    {
        _builder.AppendLiteral(str);
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        _builder.AppendLiteral(literal.Value);
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "") => _builder.AppendFormatted(
        name.GetNameFromCallerArgumentExpression<T>(),
        value,
        alignment, 
        format);

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        var result = FormattedLogValuesBuilder.BuildAndRelease(_builder);
        _builder = null!;
        return result;
    }
}

