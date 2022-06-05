#nullable enable
using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog;

/// <summary>
/// Provides extensions methods for <see cref="ILogger" /> to enable structured logging using interpolated strings.
/// </summary>
public static class LoggerExtensions
{
	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Verbose" /> level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void VerboseInterpolated(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref VerboseLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Verbose" /> level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void VerboseInterpolated(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref VerboseLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Debug" /> level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void DebugInterpolated(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Debug" /> level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void DebugInterpolated(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Information" /> level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void InformationInterpolated(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Information" /> level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void InformationInterpolated(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Warning" /> level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void WarningInterpolated(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Warning" /> level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void WarningInterpolated(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Error" /> level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void ErrorInterpolated(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Error" /> level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void ErrorInterpolated(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Fatal" /> level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void FatalInterpolated(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref FatalLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the <see cref="LogEventLevel.Fatal" /> level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void FatalInterpolated(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref FatalLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the specified level.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
	/// <param name="logEventLevel">The level of the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void WriteInterpolated(
		this ILogger logger,
		LogEventLevel logEventLevel,
		[InterpolatedStringHandlerArgument("logger", "logEventLevel")] ref LogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset();
			logger.Write(logEvent);
		}
	}

	/// <summary>
	/// Write a log event with the specified level and associated exception.
	/// </summary>
	/// <param name="logger">The <see cref="ILogger" /> to write to.</param>
	/// <param name="logEventLevel">The level of the event.</param>
    /// <param name="exception">The exception related to the event.</param>
    /// <param name="handler">The interpolated string of the log event message.</param>
	public static void WriteInterpolated(
		this ILogger logger,
		LogEventLevel logEventLevel,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger", "logEventLevel")] ref LogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEventAndReset(exception);
			logger.Write(logEvent);
		}
	}

}