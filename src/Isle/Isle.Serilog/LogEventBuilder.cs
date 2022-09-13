using System.Runtime.CompilerServices;
using Isle.Serilog.Configuration;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog;

internal abstract class LogEventBuilder
{
    private readonly object?[] _pooledValueArray = new object?[1];

    [ThreadStatic]
    private static LogEventBuilder? _cachedInstance;

    public static LogEventBuilder Acquire(int literalLength, int formattedCount, ILogger logger)
    {
        bool enableCaching = SerilogConfiguration.Current.EnableMessageTemplateCaching;
        var builder = _cachedInstance;
        if (builder != null)
        {
            _cachedInstance = null;
            if (enableCaching != builder.IsCaching)
            {
                builder = CreateFormattedLogValuesBuilder(enableCaching);
            }
        }
        else
        {
            builder = CreateFormattedLogValuesBuilder(enableCaching);
        }

        builder.Initialize(literalLength, formattedCount, logger);
        return builder;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static LogEventBuilder CreateFormattedLogValuesBuilder(bool enableCaching) => 
            enableCaching ? new CachingLogEventBuilder() : new SimpleLogEventBuilder();
    }

    public static LogEvent BuildAndRelease(LogEventBuilder builder, LogEventLevel level, Exception? exception = null)
    {
        var result = builder.BuildAndReset(level, exception);
        _cachedInstance = builder;
        return result;
    }

    public abstract bool IsCaching { get; }

    protected abstract void Initialize(int literalLength, int formattedCount, ILogger logger);

    protected abstract LogEvent BuildAndReset(LogEventLevel level, Exception? exception = null);

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