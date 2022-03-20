using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Isle.Extensions.Logging.Tests.Logging
{
    public static class TestLoggerExtensions
    {
        public static ILoggingBuilder AddTest(this ILoggingBuilder builder, Action<TestLoggerOptions> configure)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TestLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<TestLoggerOptions, TestLoggerProvider>(builder.Services);

            builder.Services.Configure(configure);

            return builder;
        }
    }
}
