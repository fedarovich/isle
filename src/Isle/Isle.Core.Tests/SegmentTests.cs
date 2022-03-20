using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Isle.Core.Tests;

public class SegmentTests
{
    [Test]
    public void AlignmentSegment([Values(-3, 0, 5)] int alignment)
    {
        var segment = new Segment(alignment);
        segment.IsLiteral.Should().BeFalse();
        segment.Alignment.Should().Be(alignment);
    }

    [Test]
    public void LiteralSegment([Values(0, 3)] int start, [Values(0, 5)] int length)
    {
        var segment = new Segment(start, length);
        segment.IsLiteral.Should().BeTrue();
        segment.Start.Should().Be(start);
        segment.Length.Should().Be(length);
    }

    [Test]
    public void Grow([Values(0, 3)] int start, [Values(0, 5)] int length, [Values(0, 11)] int increment)
    {
        var segment = new Segment(start, length);
        Segment.Grow(ref segment, increment);
        segment.IsLiteral.Should().BeTrue();
        segment.Start.Should().Be(start);
        segment.Length.Should().Be(length + increment);
    }
}