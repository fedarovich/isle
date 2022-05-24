using System;
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
}