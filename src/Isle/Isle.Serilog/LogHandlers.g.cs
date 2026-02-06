#nullable enable
using System.Runtime.CompilerServices;
using Isle.Serilog.Configuration;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog;


/// <summary>
/// Interpolated string handler for LogVerbose() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct VerboseLogInterpolatedStringHandler
{
    private LogEventBuilder _builder = null!;
    
    /// <summary>
    /// Creates a new instance of <see cref="VerboseLogInterpolatedStringHandler" />.
    /// </summary>
    public VerboseLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogEventLevel.Verbose);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(LogEventLevel.Verbose, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Verbose, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Verbose, exception); // Devirtualize the call
        }
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
    private LogEventBuilder _builder = null!;
    
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
        isEnabled = logger.IsEnabled(LogEventLevel.Debug);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(LogEventLevel.Debug, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Debug, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Debug, exception); // Devirtualize the call
        }
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
    private LogEventBuilder _builder = null!;
    
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
        isEnabled = logger.IsEnabled(LogEventLevel.Information);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(LogEventLevel.Information, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Information, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Information, exception); // Devirtualize the call
        }
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
    private LogEventBuilder _builder = null!;
    
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
        isEnabled = logger.IsEnabled(LogEventLevel.Warning);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(LogEventLevel.Warning, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Warning, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Warning, exception); // Devirtualize the call
        }
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
    private LogEventBuilder _builder = null!;
    
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
        isEnabled = logger.IsEnabled(LogEventLevel.Error);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(LogEventLevel.Error, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Error, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Error, exception); // Devirtualize the call
        }
        _builder = null!;
        return result;
    }
}


/// <summary>
/// Interpolated string handler for LogFatal() method.
/// </summary>
/// <remarks>
/// This class should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct FatalLogInterpolatedStringHandler
{
    private LogEventBuilder _builder = null!;
    
    /// <summary>
    /// Creates a new instance of <see cref="FatalLogInterpolatedStringHandler" />.
    /// </summary>
    public FatalLogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(LogEventLevel.Fatal);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(LogEventLevel.Fatal, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Fatal, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(LogEventLevel.Fatal, exception); // Devirtualize the call
        }
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
    private LogEventBuilder _builder = null!;
    private LogEventLevel _logEventLevel = default;
    
    /// <summary>
    /// Creates a new instance of <see cref="LogInterpolatedStringHandler" />.
    /// </summary>
    public LogInterpolatedStringHandler(
        int literalLength, 
        int formattedCount,
        ILogger logger,
        LogEventLevel logEventLevel,
        out bool isEnabled
    )
    {
        isEnabled = logger.IsEnabled(logEventLevel);
        if (isEnabled)
        {
            _builder = LogEventBuilder.Acquire(literalLength, formattedCount, logger);
            _logEventLevel = logEventLevel;
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
        if (!string.IsNullOrEmpty(str))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <see cref="LiteralValue" /> to the template string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in LiteralValue literal)
    {
        if (!string.IsNullOrEmpty(literal.Value))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="namedLogValue" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null)
    {
        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue); // Devirtualize the call
            }
        }
        else
        {
            if (SerilogConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue, alignment, format);
            }
            else if (SerilogConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleLogEventBuilder>(_builder).AppendFormatted(namedLogValue, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal LogEvent GetLogEventAndReset(Exception? exception = null)
    {
        LogEvent result;
        if (SerilogConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset(_logEventLevel, exception);
        }
        else if (SerilogConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingLogEventBuilder>(_builder).BuildAndReset(_logEventLevel, exception); // Devirtualize the call
        }
        else
        {
            result = Unsafe.As<SimpleLogEventBuilder>(_builder).BuildAndReset(_logEventLevel, exception); // Devirtualize the call
        }
        _builder = null!;
        return result;
    }
}

