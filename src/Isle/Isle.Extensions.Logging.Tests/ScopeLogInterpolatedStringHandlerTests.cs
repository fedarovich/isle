using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests;

[TestFixtureSource(nameof(FixtureArgs))]
public class ScopeLogInterpolatedStringHandlerTests : BaseFixture
{
    public ScopeLogInterpolatedStringHandlerTests(LogLevel minLogLevel, bool enableCaching) : base(minLogLevel, enableCaching)
    {
    }

    [Test]
    public void CreateHandler()
    {
        var handler = new ScopeLogInterpolatedStringHandler(0, 0, Logger);
        handler.GetFormattedLogValuesAndReset();
    }

    [Test]
    public void AppendLiteral([ValueSource(nameof(Literals))] string? literal)
    {
        var handler = new ScopeLogInterpolatedStringHandler(3, 0, Logger);
        handler.AppendLiteral(literal);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", literal ?? ""));
        values.ToString().Should().Be(literal ?? "");
    }

    [Test]
    public void AppendLiteralWithBraces()
    {
        var handler = new ScopeLogInterpolatedStringHandler(3, 0, Logger);
        handler.AppendLiteral("A{B}C");
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", "A{{B}}C"));
        values.ToString().Should().Be("A{B}C");
    }

    [Test]
    public void AppendLiterals()
    {
        var handler = new ScopeLogInterpolatedStringHandler(5, 0, Logger);
        handler.AppendLiteral("A");
        handler.AppendLiteral("B");
        handler.AppendLiteral("C");
        handler.AppendLiteral(null);
        handler.AppendLiteral("D");
        handler.AppendLiteral("");
        handler.AppendLiteral("E");
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(1);
        values[0].Should().Be(new KeyValuePair<string, object>("{OriginalFormat}", "ABCDE"));
        values.ToString().Should().Be("ABCDE");
    }

    #region AppendFormatted with Scalar

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value)
    {
        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
        handler.AppendFormatted(value);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object?>(nameof(value), value),
                new KeyValuePair<string, object?>("{OriginalFormat}", "{value}")
            });
        values.ToString().Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
    }

    [Test]
    public void AppendFormattedScalar<T>([ValueSource(nameof(Scalars))] T value, [Values(-6, 5)] int alignment)
    {
        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new[] { 1, 2, 3 };

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new[] { 1, 2, 3 };

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new[] { 1, 2, 3 };

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new[] { 1, 2, 3 };

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new TestObject(1, 2);

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new TestObject(1, 2);

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new TestObject(1, 2);

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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
        var value = new TestObject(1, 2);

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
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

    [Test]
    public void AppendFormattedNamedComplexType([Values(-6, 5)] int alignment, [Values("00000", "N")] string format)
    {
        var value = new TestObject(1, 2);
        var name = "NAME";

        var handler = new ScopeLogInterpolatedStringHandler(0, 1, Logger);
        handler.AppendFormatted(value.Named(name), alignment, format);
        var values = handler.GetFormattedLogValuesAndReset();
        values.Count.Should().Be(2);
        values.Should().BeEquivalentTo(
            new[]
            {
                new KeyValuePair<string, object>("@" + name, value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@" + name + "," + alignment + ":" + format + "}")
            });
        values.ToString().Should().Be(value.ToString());
    }

    #endregion
}