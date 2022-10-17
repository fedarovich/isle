using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Isle.Core.Tests;

public class ValueStringBuilderTests
{
    [Test]
    public void Ctor_Default_CanAppend()
    {
        var vsb = default(ValueStringBuilder);
        vsb.Length.Should().Be(0);

        vsb.Append('a');
        vsb.Length.Should().Be(1);
        vsb.ToString().Should().Be("a");
    }

    [Test]
    public void Ctor_Span_CanAppend()
    {
        var vsb = new ValueStringBuilder(new char[1]);
        vsb.Length.Should().Be(0);

        vsb.Append('a');
        vsb.Length.Should().Be(1);
        vsb.ToString().Should().Be("a");
    }

    [Test]
    public void Ctor_InitialCapacity_CanAppend()
    {
        var vsb = new ValueStringBuilder(1);
        vsb.Length.Should().Be(0);

        vsb.Append('a');
        vsb.Length.Should().Be(1);
        vsb.ToString().Should().Be("a");
    }

    [Test]
    public void Append_Char_MatchesStringBuilder()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();
        for (int i = 1; i <= 100; i++)
        {
            sb.Append((char)i);
            vsb.Append((char)i);
        }

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

    [Test]
    public void Append_String_MatchesStringBuilder()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();
        for (int i = 1; i <= 100; i++)
        {
            string s = i.ToString();
            sb.Append(s);
            vsb.Append(s);
        }

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

    [TestCase(0, 4 * 1024 * 1024)]
    [TestCase(1025, 4 * 1024 * 1024)]
    [TestCase(3 * 1024 * 1024, 6 * 1024 * 1024)]
    public void Append_String_Large_MatchesStringBuilder(int initialLength, int stringLength)
    {
        var sb = new StringBuilder(initialLength);
        var vsb = new ValueStringBuilder(new char[initialLength]);

        string s = new string('a', stringLength);
        sb.Append(s);
        vsb.Append(s);

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

    [Test]
    public void Append_CharInt_MatchesStringBuilder()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();
        for (int i = 1; i <= 100; i++)
        {
            sb.Append((char)i, i);
            vsb.Append((char)i, i);
        }

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

    [Test]
    public unsafe void Append_PtrInt_MatchesStringBuilder()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();
        for (int i = 1; i <= 100; i++)
        {
            string s = i.ToString();
            fixed (char* p = s)
            {
                sb.Append(p, s.Length);
                vsb.Append(p, s.Length);
            }
        }

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

    [Test]
    public void AppendSpan_DataAppendedCorrectly()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();

        for (int i = 1; i <= 1000; i++)
        {
            string s = i.ToString();

            sb.Append(s);

            Span<char> span = vsb.AppendSpan(s.Length);
            vsb.Length.Should().Be(sb.Length);

            s.AsSpan().CopyTo(span);
        }

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

    [Test]
    public void Insert_IntCharInt_MatchesStringBuilder()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();
        var rand = new Random(42);

        for (int i = 1; i <= 100; i++)
        {
            int index = rand.Next(sb.Length);
            sb.Insert(index, new string((char)i, 1), i);
            vsb.Insert(index, (char)i, i);
        }

        vsb.Length.Should().Be(sb.Length);
        vsb.ToString().Should().Be(sb.ToString());
    }

#if NETCOREAPP || NETSTANDARD2_1
    [Test]
    public void AsSpan_ReturnsCorrectValue_DoesntClearBuilder()
    {
        var sb = new StringBuilder();
        var vsb = new ValueStringBuilder();

        for (int i = 1; i <= 100; i++)
        {
            string s = i.ToString();
            sb.Append(s);
            vsb.Append(s);
        }

        var resultString = new string(vsb.AsSpan());
        resultString.Should().Be(sb.ToString());

        sb.Length.Should().NotBe(0);
        vsb.Length.Should().Be(sb.Length);
    }
#endif

    [Test]
    public void ToString_ClearsBuilder_ThenReusable()
    {
        const string Text1 = "test";
        var vsb = new ValueStringBuilder();

        vsb.Append(Text1);
        vsb.Length.Should().Be(Text1.Length);

        string s = vsb.ToString();
        s.Should().Be(Text1);

        vsb.Length.Should().Be(0);
        vsb.ToString().Should().Be(string.Empty);
        Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

        const string Text2 = "another test";
        vsb.Append(Text2);
        vsb.Length.Should().Be(Text2.Length);
        vsb.ToString().Should().Be(Text2);
    }

    [Test]
    public void TryCopyTo_FailsWhenDestinationIsTooSmall_SucceedsWhenItsLargeEnough()
    {
        var vsb = new ValueStringBuilder();

        const string Text = "expected text";
        vsb.Append(Text);
        vsb.Length.Should().Be(Text.Length);

        Span<char> dst = new char[Text.Length - 1];
        Assert.False(vsb.TryCopyTo(dst, out int charsWritten));
        charsWritten.Should().Be(0);
        vsb.Length.Should().Be(0);
    }

#if NETCOREAPP || NETSTANDARD2_1
    [Test]
    public void TryCopyTo_ClearsBuilder_ThenReusable()
    {
        const string Text1 = "test";
        var vsb = new ValueStringBuilder();

        vsb.Append(Text1);
        vsb.Length.Should().Be(Text1.Length);

        Span<char> dst = new char[Text1.Length];
        Assert.True(vsb.TryCopyTo(dst, out int charsWritten));
        charsWritten.Should().Be(Text1.Length);
        new string(dst).Should().Be(Text1);

        vsb.Length.Should().Be(0);
        vsb.ToString().Should().Be(string.Empty);
        Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

        const string Text2 = "another test";
        vsb.Append(Text2);
        vsb.Length.Should().Be(Text2.Length);
        vsb.ToString().Should().Be(Text2);
    }
#endif

    [Test]
    public void Dispose_ClearsBuilder_ThenReusable()
    {
        const string Text1 = "test";
        var vsb = new ValueStringBuilder();

        vsb.Append(Text1);
        vsb.Length.Should().Be(Text1.Length);

        vsb.Dispose();

        vsb.Length.Should().Be(0);
        vsb.ToString().Should().Be(string.Empty);
        Assert.True(vsb.TryCopyTo(Span<char>.Empty, out _));

        const string Text2 = "another test";
        vsb.Append(Text2);
        vsb.Length.Should().Be(Text2.Length);
        vsb.ToString().Should().Be(Text2);
    }

    [Test]
    public void Indexer()
    {
        const string Text1 = "foobar";
        var vsb = new ValueStringBuilder();

        vsb.Append(Text1);

        vsb[3].Should().Be('b');
        vsb[3] = 'c';
        vsb[3].Should().Be('c');
    }

    [Test]
    public void EnsureCapacity_IfRequestedCapacityWins()
    {
        // Note: constants used here may be dependent on minimal buffer size
        // the ArrayPool is able to return.
        var builder = new ValueStringBuilder(stackalloc char[32]);

        builder.EnsureCapacity(65);

        builder.Capacity.Should().Be(128);
    }

    [Test]
    public void EnsureCapacity_IfBufferTimesTwoWins()
    {
        var builder = new ValueStringBuilder(stackalloc char[32]);

        builder.EnsureCapacity(33);

        builder.Capacity.Should().Be(64);
    }

    [Test]
    public void EnsureCapacity_NoAllocIfNotNeeded()
    {
        // Note: constants used here may be dependent on minimal buffer size
        // the ArrayPool is able to return.
        var builder = new ValueStringBuilder(stackalloc char[64]);

        builder.EnsureCapacity(16);

        builder.Capacity.Should().Be(64);
    }
}