using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests;

[TestFixtureSource(nameof(FixtureArgs))]
public class LoggerExtensionsTestsProto : BaseFixture
{
    public LoggerExtensionsTestsProto(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void Log_Literal([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        string message = "Test";
        Logger.Log(logLevel, message);

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }
}