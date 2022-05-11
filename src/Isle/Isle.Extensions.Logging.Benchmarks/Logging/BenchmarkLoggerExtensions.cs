using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Isle.Extensions.Logging.Benchmarks.Logging;

public static class BenchmarkLoggerExtensions
{
    public static ILoggingBuilder AddBenchmark(this ILoggingBuilder builder, Action<BenchmarkLoggerOptions> configure)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, BenchmarkLoggerProvider>());
        LoggerProviderOptions.RegisterProviderOptions<BenchmarkLoggerOptions, BenchmarkLoggerProvider>(builder.Services);

        builder.Services.Configure(configure);

        return builder;
    }
}