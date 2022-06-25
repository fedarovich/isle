using Serilog;
using Serilog.Configuration;

namespace Isle.Serilog.Benchmarks;

public static class BenchmarkSinkExtensions
{
    public static LoggerConfiguration BenchmarkSink(
        this LoggerSinkConfiguration loggerConfiguration)
    {
        return loggerConfiguration.Sink(new BenchmarkSink());
    }
}