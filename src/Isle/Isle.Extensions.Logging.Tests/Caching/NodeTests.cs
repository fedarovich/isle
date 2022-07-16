using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Isle.Extensions.Logging.Caching;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests.Caching;

internal abstract class NodeTests<TNode> where TNode : Node
{
    protected TNode ParentNode { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        ParentNode = CreateParentNode();
    }

    [TearDown]
    public void TearDown()
    {
        NodeCache.Instance.Reset();
        ParentNode.Reset();
        ParentNode = null!;
    }

    protected abstract TNode CreateParentNode();

    [Test]
    public void GetOrAddLiteralNode()
    {
        var literal = Guid.NewGuid().ToString();

        var literalNode = ParentNode.GetOrAddLiteralNode(literal);
        literalNode.RawLiteral.Should().Be(literal);
        literalNode.EscapedLiteral.Should().Be(literal);
        literalNode.Depth.Should().Be(2);
        literalNode.Parent.Should().Be(ParentNode);

        var literalNode2 = ParentNode.GetOrAddLiteralNode(literal);
        literalNode2.Should().BeSameAs(literalNode);
    }


    [Test]
    public void GetOrAddLiteralNodeWithEscaping()
    {
        var literal = Guid.NewGuid().ToString("B");

        var literalNode = ParentNode.GetOrAddLiteralNode(literal);
        literalNode.RawLiteral.Should().Be(literal);
        literalNode.EscapedLiteral.Should().Be("{" + literal + "}");
        literalNode.Depth.Should().Be(2);
        literalNode.Parent.Should().Be(ParentNode);

        var literalNode2 = ParentNode.GetOrAddLiteralNode(literal);
        literalNode2.Should().BeSameAs(literalNode);
    }

    [Test]
    public void GetOrAddHoleNode()
    {
        var name = "x";

        var holeNode = ParentNode.GetOrAddHoleNode(name);
        holeNode.Name.Should().Be(name);
        holeNode.Depth.Should().Be(2);
        holeNode.Parent.Should().Be(ParentNode);

        var holeNode2 = ParentNode.GetOrAddHoleNode(name);
        holeNode2.Should().BeSameAs(holeNode);
    }

    [Test]
    public void GetOrAddFormattedHoleNode()
    {
        var name = "x";
        var format = "F2";
        var alignment = 5;

        var holeNode = ParentNode.GetOrAddFormattedHoleNode(name, alignment, format);
        holeNode.Name.Should().Be(name);
        holeNode.Alignment.Should().Be(alignment);
        holeNode.Format.Should().Be(format);
        holeNode.Depth.Should().Be(2);
        holeNode.Parent.Should().Be(ParentNode);

        var holeNode2 = ParentNode.GetOrAddFormattedHoleNode(name, alignment, format);
        holeNode2.Should().BeSameAs(holeNode);
    }


    [Test]
    [Repeat(10)]
    public void GetOrAddLiteralNodeMultithreaded()
    {
        var literal = Guid.NewGuid().ToString();

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new LiteralNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddLiteralNode(literal);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        var firstNode = nodes[0];
        foreach (var node in nodes.Skip(1))
        {
            node.Should().BeSameAs(firstNode);
        }
    }

    [Test]
    [Repeat(10)]
    public void GetOrAddLiteralNodesMultithreaded()
    {
        string[] literals =
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new LiteralNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                var text = literals[index & 1];
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddLiteralNode(text);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        for (int i = 2; i < threadCount; i++)
        {
            nodes[i].Should().BeSameAs(nodes[i & 1]);
        }
    }

    [Test]
    [Repeat(10)]
    public void GetOrAddHoleNodeMultithreaded()
    {
        var name = "X" + Guid.NewGuid().ToString("N");

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new HoleNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddHoleNode(name);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        var firstNode = nodes[0];
        foreach (var node in nodes.Skip(1))
        {
            node.Should().BeSameAs(firstNode);
        }
    }

    [Test]
    [Repeat(10)]
    public void GetOrAddHoleNodesMultithreaded()
    {
        string[] names =
        {
            "X" + Guid.NewGuid().ToString("N"),
            "Y" + Guid.NewGuid().ToString("N")
        };

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new HoleNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                var name = names[index & 1];
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddHoleNode(name);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        for (int i = 2; i < threadCount; i++)
        {
            nodes[i].Should().BeSameAs(nodes[i & 1]);
        }
    }

    [Test]
    [Repeat(10)]
    public void GetOrAddFormattedHoleNodeMultithreaded()
    {
        var name = "X" + Guid.NewGuid().ToString("N");
        var format = "F3";
        var alignment = -6;

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new FormattedHoleNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddFormattedHoleNode(name, alignment, format);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        var firstNode = nodes[0];
        foreach (var node in nodes.Skip(1))
        {
            node.Should().BeSameAs(firstNode);
        }
    }

    [Test]
    [Repeat(10)]
    public void GetOrAddFormattedHoleNodesMultithreaded()
    {
        string[] names =
        {
            "X" + Guid.NewGuid().ToString("N"),
            "Y" + Guid.NewGuid().ToString("N")
        };
        var format = "F3";
        var alignment = -6;

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new FormattedHoleNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                var name = names[index & 1];
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddFormattedHoleNode(name, alignment, format);
            }))
            .ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        for (int i = 2; i < threadCount; i++)
        {
            nodes[i].Should().BeSameAs(nodes[i & 1]);
        }
    }
}