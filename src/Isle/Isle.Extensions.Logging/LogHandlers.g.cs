#nullable enable
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Isle.Configuration;
using Isle.Extensions.Logging.Configuration;

namespace Isle.Extensions.Logging;


/// <summary>
/// Interpolated string handler for the <c>LogTrace()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct TraceLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="TraceLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>LogDebug()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct DebugLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="DebugLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>LogInformation()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct InformationLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="InformationLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>LogWarning()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct WarningLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="WarningLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>LogError()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct ErrorLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="ErrorLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>LogCritical()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct CriticalLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="CriticalLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>Log()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct LogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="LogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
    /// <param name="logLevel">The level of the log message.</param>
    /// <param name="isEnabled">
    /// When this method returns, contains <see langword="true"/> if the log level is enabled for the <paramref name="logger"/>;
    /// otherwise, <see langword="false"/>, in which case the interpolation expressions are not evaluated.
    /// </param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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
/// Interpolated string handler for the <c>BeginScopeInterpolated()</c> extension methods
/// of <see cref="LoggerExtensions"/>.
/// </summary>
/// <remarks>
/// This type is used by the C# compiler. It should not be used directly in your code.
/// </remarks>
[InterpolatedStringHandler]
public ref partial struct ScopeLogInterpolatedStringHandler
{
    private IFormattedLogValuesBuilder _builder = null!;

    /// <summary>
    /// Creates a new instance of <see cref="ScopeLogInterpolatedStringHandler" />.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="logger">The logger the message will be written to.</param>
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
    /// <param name="str">The literal part of the interpolated string.</param>
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
    /// <param name="literal">The literal value to be added to the message template as text.</param>
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value of the interpolation expression.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
    /// <param name="nameExpression">
    /// The source code of the interpolation expression, captured by the compiler. It is used to produce the property name.
    /// </param>
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
    /// <param name="namedLogValue">The value with an explicit name, created by <see cref="Isle.Extensions.LoggingExtensions.Named{T}(T, string)"/>.</param>
    /// <param name="alignment">The alignment specified in the interpolation expression, or 0 if none.</param>
    /// <param name="format">The format string specified in the interpolation expression, or <see langword="null"/> if none.</param>
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

