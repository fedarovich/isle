using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Isle.Extensions.Logging.Benchmarks.Logging;

[ProviderAlias("Benchmark")]
internal class BenchmarkLoggerProvider : ILoggerProvider
{
    private readonly IOptionsMonitor<BenchmarkLoggerOptions> _options;
    private readonly ConcurrentDictionary<string, BenchmarkLogger> _loggers = new();

    public BenchmarkLoggerProvider(IOptionsMonitor<BenchmarkLoggerOptions> options)
    {
        _options = options;
    }

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(
            categoryName,
            static (name, options) => new BenchmarkLogger(name, options),
            _options);
    }
}