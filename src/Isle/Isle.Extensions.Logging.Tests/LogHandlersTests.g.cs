#nullable enable
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using FluentAssertions;

namespace Isle.Extensions.Logging.Tests;


[TestFixtureSource(nameof(FixtureArgs))]
public class TraceLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogLevel logLevel = LogLevel.Trace;

    public TraceLogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new TraceLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new TraceLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new TraceLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class DebugLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogLevel logLevel = LogLevel.Debug;

    public DebugLogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new DebugLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new DebugLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new DebugLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class InformationLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogLevel logLevel = LogLevel.Information;

    public InformationLogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new InformationLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new InformationLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new InformationLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class WarningLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogLevel logLevel = LogLevel.Warning;

    public WarningLogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new WarningLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new WarningLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new WarningLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class ErrorLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogLevel logLevel = LogLevel.Error;

    public ErrorLogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new ErrorLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new ErrorLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new ErrorLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class CriticalLogInterpolatedStringHandlerTests : BaseFixture
{
    private const LogLevel logLevel = LogLevel.Critical;

    public CriticalLogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new CriticalLogInterpolatedStringHandler(0, 0, Logger,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new CriticalLogInterpolatedStringHandler(3, 0, Logger,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType()
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new CriticalLogInterpolatedStringHandler(0, 1, Logger,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}


[TestFixtureSource(nameof(FixtureArgs))]
public class LogInterpolatedStringHandlerTests : BaseFixture
{

    public LogInterpolatedStringHandlerTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }

    [Test]
    public void CreateHandler( [ValueSource(nameof(LogLevels))] LogLevel logLevel )
    {
        var handler = new LogInterpolatedStringHandler(0, 0, Logger,  logLevel,  out bool isEnabled);
        handler.IsEnabled.Should().Be(logLevel >= MinLogLevel);
        isEnabled.Should().Be(logLevel >= MinLogLevel);
        if (logLevel >= MinLogLevel)
        {
            handler.GetFormattedLogValuesAndReset();
        }
    }

    [Test]
    public void AppendLiteral( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [ValueSource(nameof(Literals))] string? literal)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new LogInterpolatedStringHandler(3, 0, Logger,  logLevel,  out _);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [ValueSource(nameof(Scalars))] T value)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new []
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.IsEnabled.Should().BeTrue();
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [ValueSource(nameof(Scalars))] T value, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value:" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:" + format + "}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [ValueSource(nameof(Scalars))] T value, [Values(-3, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0," + alignment + ":" + format + "}", value));
    }

    #endregion

    #region AppendFormatted with Collection

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogLevels))] LogLevel logLevel )
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    [Test]
    public void AppendFormattedCollection( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new[] { 1, 2, 3 };

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be("1, 2, 3");
    }

    #endregion

    #region AppendFormat with Complex Object

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogLevels))] LogLevel logLevel )
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values(-6, 5)] int alignment)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, alignment);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, format: format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value:" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    [Test]
    public void AppendFormattedComplexType( [ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        Assume.That(logLevel >= MinLogLevel);

        var value = new TestObject(1, 2);

        var handler = new LogInterpolatedStringHandler(0, 1, Logger,  logLevel,  out _);
        handler.AppendFormatted(value, alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + nameof(value), value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}

