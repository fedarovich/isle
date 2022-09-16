using System.Runtime.CompilerServices;
using Isle.Serilog.Configuration;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog;

internal abstract class LogEventBuilder
{
    private readonly object?[] _pooledValueArray = new object?[1];

    // We do not want to inline this method as it will add about additional 80 bytes of code each time we use logging.
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static LogEventBuilder Acquire(int literalLength, int formattedCount, ILogger logger)
    {
        bool enableCaching = SerilogConfiguration.Current.EnableMessageTemplateCaching;

        LogEventBuilder builder = enableCaching
            ? CachingLogEventBuilder.AcquireAndInitialize(formattedCount, logger)
            : SimpleLogEventBuilder.AcquireAndInitialize(literalLength, formattedCount, logger);

        return builder;
    }

    public abstract bool IsCaching { get; }

    public abstract LogEvent BuildAndReset(LogEventLevel level, Exception? exception = null);

    public abstract void AppendLiteral(string? str);

    public virtual void AppendLiteralValue(in LiteralValue literalValue)
    {
        AppendLiteral(literalValue.Value);
    }

    public abstract void AppendFormatted<T>(string name, T value, int alignment, string? format);

    public abstract void AppendFormatted(in NamedLogValue namedLogValue, int alignment, string? format);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected object?[] AcquirePooledValueArray(object? value)
    {
        _pooledValueArray[0] = value;
        return _pooledValueArray;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void ReleasePooledValueArray()
    {
        _pooledValueArray[0] = null;
    }
}