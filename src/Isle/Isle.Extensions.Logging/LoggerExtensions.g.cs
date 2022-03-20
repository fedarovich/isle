#nullable enable
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging;

public static class LoggerExtensions
{
	private static readonly Func<FormattedLogValues, Exception?, string> _messageFormatter = MessageFormatter;

	public static void LogTrace(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref TraceLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Trace, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogTrace(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref TraceLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Trace, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogTrace(
		this ILogger logger,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger")] ref TraceLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Trace, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogTrace(
		this ILogger logger,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref TraceLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Trace, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogDebug(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Debug, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogDebug(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Debug, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogDebug(
		this ILogger logger,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Debug, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogDebug(
		this ILogger logger,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref DebugLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Debug, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogInformation(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Information, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogInformation(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Information, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogInformation(
		this ILogger logger,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Information, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogInformation(
		this ILogger logger,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref InformationLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Information, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogWarning(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Warning, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogWarning(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Warning, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogWarning(
		this ILogger logger,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Warning, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogWarning(
		this ILogger logger,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref WarningLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Warning, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogError(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Error, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogError(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Error, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogError(
		this ILogger logger,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Error, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogError(
		this ILogger logger,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref ErrorLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Error, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogCritical(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref CriticalLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Critical, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogCritical(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref CriticalLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Critical, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogCritical(
		this ILogger logger,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger")] ref CriticalLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Critical, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void LogCritical(
		this ILogger logger,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref CriticalLogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, LogLevel.Critical, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void Log(
		this ILogger logger,
		LogLevel logLevel,
		[InterpolatedStringHandlerArgument("logger", "logLevel")] ref LogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, logLevel, default, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void Log(
		this ILogger logger,
		LogLevel logLevel,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger", "logLevel")] ref LogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, logLevel, default, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void Log(
		this ILogger logger,
		LogLevel logLevel,
		EventId eventId,
		[InterpolatedStringHandlerArgument("logger", "logLevel")] ref LogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, logLevel, eventId, null, handler.GetFormattedLogValuesAndReset());
		}
	}

	public static void Log(
		this ILogger logger,
		LogLevel logLevel,
		EventId eventId,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger", "logLevel")] ref LogInterpolatedStringHandler handler
	)
	{
		if (handler.IsEnabled)
		{
			Log(logger, logLevel, eventId, exception, handler.GetFormattedLogValuesAndReset());
		}
	}

	private static void Log(ILogger logger, LogLevel logLevel, EventId eventId, Exception? exception, in FormattedLogValues formattedLogValues)
	{
		logger.Log(logLevel, eventId, formattedLogValues, exception, _messageFormatter);
	}

	public static IDisposable BeginScopeInterpolated(this ILogger logger, [InterpolatedStringHandlerArgument("logger")] ref ScopeLogInterpolatedStringHandler handler)
	{
		return logger.BeginScope(handler.GetFormattedLogValuesAndReset());
	}

	private static string MessageFormatter(FormattedLogValues state, Exception? error) => state.ToString();
}