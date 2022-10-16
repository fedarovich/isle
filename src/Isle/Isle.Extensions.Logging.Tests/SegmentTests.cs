using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests;

public class SegmentTests
{
    [Test]
    public void FormattedValueSegment([Values(null, "", "N")] string? format, [Values(-3, 0, 5)] int alignment)
    {
        var segment = new Segment(format, alignment);
        segment.Type.Should().Be(Segment.SegmentType.FormattedValue);
        segment.Format.Should().Be(format);
        segment.Alignment.Should().Be(alignment);
    }

    [Test]
    public void LiteralSegment()
    {
        const string literal = "Test";
        var segment = new Segment(literal);
        segment.Type.Should().Be(Segment.SegmentType.Literal);
        segment.Literal.Should().Be(literal);
    }

    [Test]
    public void MergeLiteralsIntoNewLiteralListSegment()
    {
        var list = new List<string>();
        var segment1 = new Segment("A");
        segment1.Type.Should().Be(Segment.SegmentType.Literal);
        
        var segment2 = new Segment(segment1, "B", list);
        segment2.Type.Should().Be(Segment.SegmentType.LiteralList);
        segment2.LiteralList.ToArray().Should().BeEquivalentTo("A", "B");

        var segment3 = new Segment(segment2, "C");
        segment3.Type.Should().Be(Segment.SegmentType.LiteralList);
        segment3.LiteralList.ToArray().Should().BeEquivalentTo("A", "B", "C");
    }

    [Test]
    public void MergeLiteralsIntoExistingLiteralListSegment()
    {
        var list = new List<string> { "X", "Y" };
        var segment1 = new Segment("A");
        segment1.Type.Should().Be(Segment.SegmentType.Literal);

        var segment2 = new Segment(segment1, "B", list);
        segment2.Type.Should().Be(Segment.SegmentType.LiteralList);
        segment2.LiteralList.ToArray().Should().BeEquivalentTo("A", "B");

        var segment3 = new Segment(segment2, "C");
        segment3.Type.Should().Be(Segment.SegmentType.LiteralList);
        segment3.LiteralList.ToArray().Should().BeEquivalentTo("A", "B", "C");
    }

    //[Test]
    //public void AlignmentSegment([Values(-3, 0, 5)] int alignment)
    //{
    //    var segment = new Segment(alignment);
    //    segment.IsLiteral.Should().BeFalse();
    //    segment.Alignment.Should().Be(alignment);
    //}

    //[Test]
    //public void LiteralSegment([Values(0, 3)] int start, [Values(0, 5)] int length)
    //{
    //    var segment = new Segment(start, length);
    //    segment.IsLiteral.Should().BeTrue();
    //    segment.Start.Should().Be(start);
    //    segment.Length.Should().Be(length);
    //}

    //[Test]
    //public void Grow([Values(0, 3)] int start, [Values(0, 5)] int length, [Values(0, 11)] int increment)
    //{
    //    var segment = new Segment(start, length);
    //    Segment.Grow(ref segment, increment);
    //    segment.IsLiteral.Should().BeTrue();
    //    segment.Start.Should().Be(start);
    //    segment.Length.Should().Be(length + increment);
    //}
}