#nullable enable
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Isle.Configuration;
using Isle.Extensions.Logging.Configuration;

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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
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
    private IFormattedLogValuesBuilder _builder = null!;

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
        if (!string.IsNullOrEmpty(str))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteral(str!);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteral(str!); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendLiteralValue(literal);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendLiteralValue(literal); // Devirtualize the call
            }
        }
    }

    /// <summary>
    /// Appends a <paramref name="value" /> to the log message.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendFormatted<T>(T value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string nameExpression = "")
    {
        var name = nameExpression.GetNameFromCallerArgumentExpression<T>();

        if (alignment == 0 && string.IsNullOrEmpty(format))
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(name, value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(name, value, alignment, format); // Devirtualize the call
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
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value); // Devirtualize the call
            }
        }
        else
        {
            if (ExtensionsLoggingConfiguration.IsResettable)
            {
                _builder.AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format);
            }
            else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
            {
                Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
            else
            {
                Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).AppendFormatted(namedLogValue.Name, namedLogValue.Value, alignment, format); // Devirtualize the call
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FormattedLogValuesBase GetFormattedLogValuesAndReset()
    {
        FormattedLogValuesBase result;
        if (ExtensionsLoggingConfiguration.IsResettable)
        {
            result = _builder.BuildAndReset();
        }
        else if (ExtensionsLoggingConfiguration.EnableMessageTemplateCaching)
        {
            result = Unsafe.As<CachingFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        else
        {
            result = Unsafe.As<SimpleFormattedLogValuesBuilder>(_builder).BuildAndReset();
        }
        _builder = null!;
        return result;
    }
}

