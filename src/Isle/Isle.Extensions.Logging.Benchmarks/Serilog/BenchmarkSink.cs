using Serilog.Core;
using Serilog.Events;

namespace Isle.Extensions.Logging.Benchmarks.Serilog;

public class BenchmarkSink : ILogEventSink
{
    private LogEvent? _logEvent;

    public void Emit(LogEvent logEvent)
    {
        _logEvent = logEvent;
    }
}