using FluentAssertions;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests;

public class SegmentTests
{
    [Test]
    public void FormattedValueSegment([Values(null, "", "N")] string? format, [Values(-3, 0, 5)] int alignment)
    {
        var segment = new Segment(format, alignment);
        segment.Alignment.Should().Be(alignment);
        segment.String.Should().Be(format);
        segment.IsLiteral.Should().BeFalse();
    }

    [Test]
    public void LiteralSegment()
    {
        const string literal = "Test";
        var segment = new Segment(literal);
        segment.Alignment.Should().Be(0);
        segment.String.Should().Be(literal);
        segment.IsLiteral.Should().BeTrue();
    }

    [Test]
    public void Default()
    {
        Segment segment = new Segment();
        segment.Alignment.Should().Be(0);
        segment.String.Should().BeNull();
        segment.IsLiteral.Should().BeFalse();
    }
}