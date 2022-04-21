using System;
using System.Collections.Generic;
using System.Linq;
using Isle.Configuration;
using Isle.Extensions.Logging.Tests.Serilog;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Isle.Extensions.Logging.Tests
{
    public abstract class SerilogBaseFixture
    {
        protected static readonly IReadOnlyList<LogLevel> LogLevels = Enum.GetValues<LogLevel>().Where(x => x != LogLevel.None).ToArray();
        
        protected static readonly IReadOnlyList<object> FixtureArgs = LogLevels.Select(x => new object[] { x }).ToArray();
        
        protected static readonly IReadOnlyList<string?> Literals = new[] { null, "A", "ABC", "ABCDE", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };

        protected static readonly IReadOnlyList<object?> Scalars = new object[] { 1000, 3.5, 2.5m, "ABC" };

        protected static readonly MessageTemplateParser Parser = new ();

        private readonly List<LogEvent> _serilogLogEvents = new();

        protected SerilogBaseFixture(LogLevel minLogLevel)
        {
            MinLogLevel = minLogLevel;
        }

        [OneTimeSetUp]
        protected virtual void OneTimeSetUp()
        {
            IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring());
            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(MinLogLevel)
                    .AddSerilog(new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .MinimumLevel.Is(ToSerilogLevel(MinLogLevel))
                        .WriteTo.TestSink(config => config.LogEvents = _serilogLogEvents)
                        .CreateLogger(), dispose: true);
            });
        }

        [OneTimeTearDown]
        protected virtual void OneTimeTearDown()
        {
            LoggerFactory.Dispose();
            IsleConfiguration.Reset();
        }

        [SetUp]
        protected virtual void Setup()
        {
            Logger = LoggerFactory.CreateLogger(GetType().FullName!);
        }

        [TearDown]
        protected virtual void TearDown()
        {
            _serilogLogEvents.Clear();
        }

        protected LogLevel MinLogLevel { get; }

        protected ILoggerFactory LoggerFactory { get; private set; } = null!;

        protected ILogger Logger { get; private set; } = null!;

        protected IReadOnlyList<LogEvent> LogEvents => _serilogLogEvents;

        protected record TestObject(int X, int Y);

        protected static LogEventLevel ToSerilogLevel(LogLevel level) =>
            level switch
            {
                LogLevel.Trace => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Critical => LogEventLevel.Fatal,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };
    }
}
