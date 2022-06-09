#nullable enable
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog.Tests;


[TestFixtureSource(nameof(FixtureArgs))]
public class VerboseLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogEventLevel logEventLevel = LogEventLevel.Verbose;

    public VerboseLogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new VerboseLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(5, 0, Logger,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new VerboseLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class DebugLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogEventLevel logEventLevel = LogEventLevel.Debug;

    public DebugLogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new DebugLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(5, 0, Logger,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class InformationLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogEventLevel logEventLevel = LogEventLevel.Information;

    public InformationLogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new InformationLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(5, 0, Logger,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class WarningLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogEventLevel logEventLevel = LogEventLevel.Warning;

    public WarningLogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new WarningLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(5, 0, Logger,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class ErrorLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogEventLevel logEventLevel = LogEventLevel.Error;

    public ErrorLogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new ErrorLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(5, 0, Logger,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class FatalLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogEventLevel logEventLevel = LogEventLevel.Fatal;

    public FatalLogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new FatalLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(5, 0, Logger,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new FatalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class LogInterpolatedStringHandlerTests : BaseFixture
{

    public LogInterpolatedStringHandlerTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel )
    {
        var handler = new LogInterpolatedStringHandler(0, 0, Logger,  logEventLevel,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        isEnabled.Should().Be(logEventLevel >= MinLogEventLevel);
        if (logEventLevel >= MinLogEventLevel)
        {
            handler.GetLogEventAndReset();
        }
    }

    [Test]
    public void AppendLiteral( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(3, 0, Logger,  logEventLevel,  out _);
        handler.AppendLiteral(literal);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be(literal ?? "");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel )
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(3, 0, Logger,  logEventLevel,  out _);
        handler.AppendLiteral("A{B}C");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("A{B}C");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel )
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(5, 0, Logger,  logEventLevel,  out _);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("ABCDE");
        logEvent.Properties.Count.Should().Be(0);
        logEvent.RenderMessage().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [ValueSource(nameof(Scalars))] T value, [Values(-5, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new ScalarValue(value))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel )
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new SequenceValue(value.Select(v => new ScalarValue(v))))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel )
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value));
    }

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values(-6, 5)] int alignment)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, alignment);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment));
    }

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, format: format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value:" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, format));
    }

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logEventLevel >= MinLogEventLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logEventLevel,  out _);
        handler.AppendFormatted(value, alignment, format);
        var logEvent = handler.GetLogEventAndReset();
        logEvent.MessageTemplate.Text.Should().Be("{value," + alignment + ":" + format + "}");
        logEvent.Properties.Count.Should().Be(1);
        logEvent.Properties.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, LogEventPropertyValue>(nameof(value), new StructureValue(new []
                {
                    new LogEventProperty(nameof(value.X), new ScalarValue(value.X)),
                    new LogEventProperty(nameof(value.Y), new ScalarValue(value.Y))
                }))
            });
        logEvent.RenderMessage().Should().Be(Format(value, alignment, format));
    }

    #endregion
}

