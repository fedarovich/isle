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

#if !NET5_0_OR_GREATER

    [Test]
    public void ListSegmentToArray()
    {
        var list = new List<string> { "A", "B", "C", "D", "E", "F" };
        var segmentList = new Segment.ListSegment<string>(list, 2, 3);
        segmentList.Length.Should().Be(3);
        segmentList.ToArray().Should().BeEquivalentTo("C", "D", "E");
    }

    [Test]
    public void ListSegmentEnumerate()
    {
        var list = new List<string> { "A", "B", "C", "D", "E", "F" };
        var segmentList = new Segment.ListSegment<string>(list, 2, 3);

        var result = new List<string>();
        foreach (var item in segmentList)
        {
            result.Add(item);
        }

        result.Count.Should().Be(3);
        result.ToArray().Should().BeEquivalentTo("C", "D", "E");
    }

    [Test]
    public void ListSegmentIterate()
    {
        var list = new List<string> { "A", "B", "C", "D", "E", "F" };
        var segmentList = new Segment.ListSegment<string>(list, 2, 3);

        var result = new List<string>();
        for (int i = 0; i < segmentList.Length; i++)
        {
            result.Add(segmentList[i]);
        }

        result.Count.Should().Be(3);
        result.ToArray().Should().BeEquivalentTo("C", "D", "E");
    }

#endif
}