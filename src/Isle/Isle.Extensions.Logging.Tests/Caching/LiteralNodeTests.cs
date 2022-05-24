using FluentAssertions;
using Isle.Extensions.Logging.Caching;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests.Caching;

[TestFixture]
internal class LiteralNodeTests : NodeTests<LiteralNode>
{
    private const string Literal = "Parent";

    protected override LiteralNode CreateParentNode() => NodeCache.Instance.GetOrAddLiteralNode(Literal);

    [Test]
    public void GetTemplateNode()
    {
        var templateNode = ParentNode.GetTemplateNode();
        templateNode.MessageTemplate.Should().Be(Literal);

        var templateNode2 = ParentNode.GetTemplateNode();
        templateNode2.Should().BeSameAs(templateNode);
    }

    [Test]
    public void Create()
    {
        var node = new LiteralNode(NodeCache.Instance, Literal);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.EscapedLiteral.Should().Be(Literal);
        node.RawLiteral.Should().Be(Literal);
    }

    [Test]
    public void CreateLongLiteral()
    {
        var longLiteral = new string('{', 2048);
        var node = new LiteralNode(NodeCache.Instance, longLiteral);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.EscapedLiteral.Should().Be(new string('{', 4096));
        node.RawLiteral.Should().Be(longLiteral);
    }
}