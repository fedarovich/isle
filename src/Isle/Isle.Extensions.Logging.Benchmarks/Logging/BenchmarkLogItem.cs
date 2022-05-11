using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging.Benchmarks.Logging;

public readonly record struct BenchmarkLogItem(
    string Category,
    LogLevel LogLevel,
    EventId EventId,
    object? State,
    Exception? Exception,
    string? Message);