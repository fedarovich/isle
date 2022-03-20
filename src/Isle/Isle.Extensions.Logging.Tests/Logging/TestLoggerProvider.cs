using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Isle.Extensions.Logging.Tests.Logging;

[ProviderAlias("Test")]
public class TestLoggerProvider : ILoggerProvider
{
    private readonly IOptionsMonitor<TestLoggerOptions> _options;
    private readonly ConcurrentDictionary<string, TestLogger> _loggers = new();

    public TestLoggerProvider(IOptionsMonitor<TestLoggerOptions> options)
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
            static (name, options) => new TestLogger(name, options), 
            _options);
    }
}