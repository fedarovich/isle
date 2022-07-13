using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Isle.Serilog.Caching;
using NUnit.Framework;
using Serilog.Parsing;

namespace Isle.Serilog.Tests.Caching;

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
    public void GetOrAddTextNode()
    {
        var text = Guid.NewGuid().ToString();

        var textNode = ParentNode.GetOrAddTextNode(text);
        textNode.Token.Text.Should().Be(text);
        textNode.RawText.Should().Be(text);
        textNode.Depth.Should().Be(2);
        textNode.Parent.Should().Be(ParentNode);

        var textNode2 = ParentNode.GetOrAddTextNode(text);
        textNode2.Should().BeSameAs(textNode);
    }


    [Test]
    public void GetOrAddTextNodeWithEscaping()
    {
        var text = Guid.NewGuid().ToString("B");

        var textNode = ParentNode.GetOrAddTextNode(text);
        textNode.Token.Text.Should().Be(text);
        textNode.RawText.Should().Be("{" + text + "}");
        textNode.Depth.Should().Be(2);
        textNode.Parent.Should().Be(ParentNode);

        var textNode2 = ParentNode.GetOrAddTextNode(text);
        textNode2.Should().BeSameAs(textNode);
    }

    [Test]
    public void GetOrAddPropertyNode()
    {
        var name = "x";

        var propertyNode = ParentNode.GetOrAddPropertyNode(name, name);
        propertyNode.Token.PropertyName.Should().Be(name);
        propertyNode.Token.Alignment.Should().Be(default(Alignment));
        propertyNode.Token.Destructuring.Should().Be(Destructuring.Default);
        propertyNode.Token.Format.Should().BeNull();
        propertyNode.RawText.Should().Be("{" + name + "}");
        propertyNode.Depth.Should().Be(2);
        propertyNode.Parent.Should().Be(ParentNode);

        var propertyNode2 = ParentNode.GetOrAddPropertyNode(name, name);
        propertyNode2.Should().BeSameAs(propertyNode);
    }

    [Test]
    public void GetOrAddPropertyNodeWithDestructuring()
    {
        var name = "x";

        var propertyNode = ParentNode.GetOrAddPropertyNode("@" + name, name);
        propertyNode.Token.PropertyName.Should().Be(name);
        propertyNode.Token.Alignment.Should().Be(default(Alignment));
        propertyNode.Token.Destructuring.Should().Be(Destructuring.Destructure);
        propertyNode.Token.Format.Should().BeNull();
        propertyNode.RawText.Should().Be("{@" + name + "}");
        propertyNode.Depth.Should().Be(2);
        propertyNode.Parent.Should().Be(ParentNode);

        var propertyNode2 = ParentNode.GetOrAddPropertyNode("@" + name, name);
        propertyNode2.Should().BeSameAs(propertyNode);
    }

    [Test]
    public void GetOrAddFormattedPropertyNode()
    {
        var name = "x";
        var format = "F2";
        var alignment = 5;

        var propertyNode = ParentNode.GetOrAddPropertyNode(name, name, alignment, format);
        propertyNode.Token.PropertyName.Should().Be(name);
        propertyNode.Token.Alignment.Should().Be(new Alignment(AlignmentDirection.Right, alignment));
        propertyNode.Token.Destructuring.Should().Be(Destructuring.Default);
        propertyNode.Token.Format.Should().Be(format);
        propertyNode.RawText.Should().Be($"{{{name},{alignment}:{format}}}");
        propertyNode.Depth.Should().Be(2);
        propertyNode.Parent.Should().Be(ParentNode);

        var holeNode2 = ParentNode.GetOrAddPropertyNode(name, name, alignment, format);
        holeNode2.Should().BeSameAs(propertyNode);
    }

    [Test]
    public void GetOrAddFormattedPropertyNodeWithStringification()
    {
        var name = "x";
        var format = "F2";
        var alignment = 5;

        var propertyNode = ParentNode.GetOrAddPropertyNode("$" + name, name, alignment, format);
        propertyNode.Token.PropertyName.Should().Be(name);
        propertyNode.Token.Alignment.Should().Be(new Alignment(AlignmentDirection.Right, alignment));
        propertyNode.Token.Destructuring.Should().Be(Destructuring.Stringify);
        propertyNode.Token.Format.Should().Be(format);
        propertyNode.RawText.Should().Be($"{{${name},{alignment}:{format}}}");
        propertyNode.Depth.Should().Be(2);
        propertyNode.Parent.Should().Be(ParentNode);

        var holeNode2 = ParentNode.GetOrAddPropertyNode("$" + name, name, alignment, format);
        holeNode2.Should().BeSameAs(propertyNode);
    }

    [Test]
    [Repeat(10)]
    public void GetOrAddTextNodeMultithreaded()
    {
        var text = Guid.NewGuid().ToString();

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new TextNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddTextNode(text);
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
    public void GetOrAddTextNodesMultithreaded()
    {
        string[] texts =
        {
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString()
        };

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new TextNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                var text = texts[index & 1];
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddTextNode(text);
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
    public void GetOrAddPropertyNodeMultithreaded()
    {
        var name = "X" + Guid.NewGuid().ToString("N");

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new PropertyNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddPropertyNode(name, name);
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
    public void GetOrAddPropertyNodesMultithreaded()
    {
        string[] names =
        {
            "X" + Guid.NewGuid().ToString("N"),
            "Y" + Guid.NewGuid().ToString("N")
        };

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new PropertyNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                var name = names[index & 1];
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddPropertyNode(name, name);
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
    public void GetOrAddFormattedPropertyNodeMultithreaded()
    {
        var name = "X" + Guid.NewGuid().ToString("N");
        var format = "F3";
        var alignment = -6;

        var threadCount = Math.Max(Environment.ProcessorCount, 4);

        using var barrier = new Barrier(threadCount);
        var nodes = new PropertyNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddPropertyNode(name, name, alignment, format);
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
    public void GetOrAddFormattedPropertyNodesMultithreaded()
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
        var nodes = new PropertyNode[threadCount];

        var threads = Enumerable.Range(0, threadCount)
            .Select(index => new Thread(() =>
            {
                var name = names[index & 1];
                // ReSharper disable once AccessToDisposedClosure
                barrier.SignalAndWait();
                nodes[index] = ParentNode.GetOrAddPropertyNode(name, name, alignment, format);
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