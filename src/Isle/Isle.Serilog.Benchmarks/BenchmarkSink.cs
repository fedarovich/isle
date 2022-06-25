using Serilog.Core;
using Serilog.Events;

namespace Isle.Serilog.Benchmarks;

public class BenchmarkSink : ILogEventSink
{
    private LogEvent? _logEvent;

    public void Emit(LogEvent logEvent)
    {
        _logEvent = logEvent;
    }
}