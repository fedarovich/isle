using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests;

public class FormattedLogValuesTests
{
    private static readonly int[] FormattedCounts = Enumerable.Range(0, 9).ToArray();

    [Test]
    public void Create0() => FormattedLogValuesBase.Create(0).Should().BeOfType<FormattedLogValues0>().Which.Count.Should().Be(1);

    [Test]
    public void Create1() => FormattedLogValuesBase.Create(1).Should().BeOfType<FormattedLogValues1>().Which.Count.Should().Be(2);

    [Test]
    public void Create2() => FormattedLogValuesBase.Create(2).Should().BeOfType<FormattedLogValues2>().Which.Count.Should().Be(3);

    [Test]
    public void Create3() => FormattedLogValuesBase.Create(3).Should().BeOfType<FormattedLogValues3>().Which.Count.Should().Be(4);

    [Test]
    public void Create4() => FormattedLogValuesBase.Create(4).Should().BeOfType<FormattedLogValues4>().Which.Count.Should().Be(5);

    [Test]
    public void Create5() => FormattedLogValuesBase.Create(5).Should().BeOfType<FormattedLogValues5>().Which.Count.Should().Be(6);

    [Test]
    public void Create6() => FormattedLogValuesBase.Create(6).Should().BeOfType<FormattedLogValues6>().Which.Count.Should().Be(7);

    [Test]
    public void Create7() => FormattedLogValuesBase.Create(7).Should().BeOfType<FormattedLogValues7>().Which.Count.Should().Be(8);

    [Test]
    public void CreateN([Values(8, 9, 10, 20, 100)] int n) => FormattedLogValuesBase.Create(n).Should().BeOfType<FormattedLogValues>().Which.Count.Should().Be(n + 1);

    [Test]
    public void FillAndEnumerate([ValueSource(nameof(FormattedCounts))] int formattedCount)
    {
        var expectedValues = Enumerable.Range(0, formattedCount + 1)
            .Select(i => new KeyValuePair<string, object?>($"k{i}", $"v{i}"))
            .ToList();

        var values = FormattedLogValuesBase.Create(formattedCount);
        values.Count.Should().Be(expectedValues.Count);
        for (int i = 0; i < expectedValues.Count; i++)
        {
            values.Values[i] = expectedValues[i];
        }

        values.Values.ToArray().Should().BeEquivalentTo(expectedValues);
        values.AsEnumerable().Should().BeEquivalentTo(expectedValues);

        for (int i = 0; i < expectedValues.Count; i++)
        {
            values.Values[i].Should().Be(expectedValues[i]);
        }

        Action negIndex = () => _ = values[-1];
        negIndex.Should().Throw<IndexOutOfRangeException>();

        Action lenIndex = () => _ = values[values.Count];
        lenIndex.Should().Throw<IndexOutOfRangeException>();
    }

    [Test]
    public void FormatNull()
    {
        var values = FormattedLogValuesBase.Create(2);
        values.Values[0] = new("x", null);
        values.Values[1] = new("y", new[] { "a", null, "b" });
        values.Values[2] = new(FormattedLogValuesBuilder.OriginalFormatName, "{x} {y}");
        values.SetSegments(new [] { new Segment(null, 0), new Segment(" "), new Segment(null, 0) });
        values.ToString().Should().Be("(null) a, (null), b");
    }
}