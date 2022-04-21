using System;
using System.Collections.Generic;
using System.Linq;
using Isle.Configuration;
using Isle.Extensions.Logging.Tests.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests
{
    public abstract class BaseFixture
    {
        protected static readonly IReadOnlyList<object> FixtureArgs = Enum.GetValues<LogLevel>().Select(x => new object[] { x }).ToArray();

        protected static readonly IReadOnlyList<LogLevel> LogLevels = Enum.GetValues<LogLevel>().Where(x => x != LogLevel.None).ToArray();

        protected static readonly IReadOnlyList<string?> Literals = new[] { null, "A", "ABC", "ABCDE", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };

        protected static readonly IReadOnlyList<object?> Scalars = new object[] { 1000, 3.5, 2.5m, "ABC" };

        private readonly List<TestLogItem> _logItems = new ();

        protected BaseFixture(LogLevel minLogLevel)
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
                        .AddTest(opt =>
                        {
                            opt.MinLogLevel = MinLogLevel;
                            opt.LogItems = _logItems;
                        });
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
            _logItems.Clear();
        }

        protected LogLevel MinLogLevel { get; }

        protected ILoggerFactory LoggerFactory { get; private set; } = null!;

        protected ILogger Logger { get; private set; } = null!;

        protected IReadOnlyList<TestLogItem> LogItems => _logItems;

        protected record TestObject(int X, int Y);
    }
}
