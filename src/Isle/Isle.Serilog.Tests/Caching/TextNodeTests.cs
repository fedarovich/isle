using FluentAssertions;
using Isle.Serilog.Caching;
using NUnit.Framework;

namespace Isle.Serilog.Tests.Caching;

internal class TextNodeTests : NodeTests<TextNode>
{
    private const string Text = "Parent";

    protected override TextNode CreateParentNode() => NodeCache.Instance.GetOrAddTextNode(Text);

    [Test]
    public void GetTemplateNode()
    {
        var templateNode = ParentNode.GetTemplateNode();
        templateNode.MessageTemplate.Text.Should().Be(Text);

        var templateNode2 = ParentNode.GetTemplateNode();
        templateNode2.Should().BeSameAs(templateNode);
    }

    [Test]
    public void Create()
    {
        var node = new TextNode(NodeCache.Instance, Text);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.RawText.Should().Be(Text);
        node.Token.Text.Should().Be(Text);
    }

    [Test]
    public void CreateLongLiteral()
    {
        var longLiteral = new string('{', 2048);
        var node = new TextNode(NodeCache.Instance, longLiteral);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.RawText.Should().Be(new string('{', 4096));
        node.Token.Text.Should().Be(longLiteral);
    }
}