using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging.Benchmarks.Logging;

public class BenchmarkLoggerOptions
{
    public LogLevel MinLogLevel { get; set; }

    public bool RenderMessage { get; set; }
}