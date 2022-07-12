using System;
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
}