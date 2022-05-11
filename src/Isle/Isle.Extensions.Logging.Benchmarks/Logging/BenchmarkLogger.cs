using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Isle.Extensions.Logging.Benchmarks.Logging;

public class BenchmarkLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IOptionsMonitor<BenchmarkLoggerOptions> _options;
    private BenchmarkLogItem _logItem;

    public BenchmarkLogger(string categoryName, IOptionsMonitor<BenchmarkLoggerOptions> options)
    {
        _categoryName = categoryName;
        _options = options;
    }

    public BenchmarkLogItem LogItem => _logItem;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        _logItem = new BenchmarkLogItem(_categoryName, logLevel, eventId, state, exception, 
            _options.CurrentValue.RenderMessage ? formatter(state, exception) : null);
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.CurrentValue.MinLogLevel;
    
    public IDisposable BeginScope<TState>(TState state) => throw new NotSupportedException();
}