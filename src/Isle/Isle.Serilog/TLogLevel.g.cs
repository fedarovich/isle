#nullable enable
using System.Runtime.CompilerServices;
using System.Text;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member


public readonly struct LogLevelVerbose: IHasLogLevel<LogLevelVerbose> {
    public static LogEventLevel GetLevel() => LogEventLevel.Verbose;
}


public readonly struct LogLevelDebug: IHasLogLevel<LogLevelDebug> {
    public static LogEventLevel GetLevel() => LogEventLevel.Debug;
}


public readonly struct LogLevelInformation: IHasLogLevel<LogLevelInformation> {
    public static LogEventLevel GetLevel() => LogEventLevel.Information;
}


public readonly struct LogLevelWarning: IHasLogLevel<LogLevelWarning> {
    public static LogEventLevel GetLevel() => LogEventLevel.Warning;
}


public readonly struct LogLevelError: IHasLogLevel<LogLevelError> {
    public static LogEventLevel GetLevel() => LogEventLevel.Error;
}


public readonly struct LogLevelFatal: IHasLogLevel<LogLevelFatal> {
    public static LogEventLevel GetLevel() => LogEventLevel.Fatal;
}



/// <summary>
/// Provides extensions methods for <see cref="ILogger" /> to enable structured logging using interpolated strings.
/// But now taking advantage of the abstract static GetLevel method on IHasLogLevel to avoid duplicate InterpolatedStringHandlers
/// </summary>
public static class TLoggerExtensions
{
    public static void Log<TLogLevel>(
		this ILogger logger,
		Exception? exception,
		ref LogISH<TLogLevel> handler, 
		[CallerArgumentExpression("handler")] string handlerExpr = "") 
		where TLogLevel : IHasLogLevel<TLogLevel> 
	{
		if (handler.IsEnabled)
		{
			var logEvent = handler.GetLogEvent(logger, handlerExpr, exception);
			logger.Write(logEvent);
		}
	}

    public static void Log<TLogLevel>(this ILogger logger, ref LogISH<TLogLevel> handler, [CallerArgumentExpression("handler")] string handlerExpr = "") where TLogLevel : IHasLogLevel<TLogLevel>
		=> Log(logger, null, ref handler, handlerExpr);

	public static void VerboseInterpolated2(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelVerbose> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, ref handler, handlerExpr);

	public static void VerboseInterpolated2(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelVerbose> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, exception, ref handler, handlerExpr);

	public static void DebugInterpolated2(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelDebug> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, ref handler, handlerExpr);

	public static void DebugInterpolated2(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelDebug> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, exception, ref handler, handlerExpr);

	public static void InformationInterpolated2(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelInformation> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, ref handler, handlerExpr);

	public static void InformationInterpolated2(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelInformation> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, exception, ref handler, handlerExpr);

	public static void WarningInterpolated2(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelWarning> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, ref handler, handlerExpr);

	public static void WarningInterpolated2(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelWarning> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, exception, ref handler, handlerExpr);

	public static void ErrorInterpolated2(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelError> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, ref handler, handlerExpr);

	public static void ErrorInterpolated2(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelError> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, exception, ref handler, handlerExpr);

	public static void FatalInterpolated2(
		this ILogger logger,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelFatal> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, ref handler, handlerExpr);

	public static void FatalInterpolated2(
		this ILogger logger,
		Exception? exception,
		[InterpolatedStringHandlerArgument("logger")] ref LogISH<LogLevelFatal> handler,
		[CallerArgumentExpression("handler")] string handlerExpr = ""
	) => Log( logger, exception, ref handler, handlerExpr);



#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}