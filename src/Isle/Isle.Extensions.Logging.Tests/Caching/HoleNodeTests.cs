using FluentAssertions;
using Isle.Extensions.Logging.Caching;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests.Caching;

internal class HoleNodeTests : NodeTests<HoleNode>
{
    private const string Name = "Var";

    protected override HoleNode CreateParentNode() => NodeCache.Instance.GetOrAddHoleNode(Name);

    [Test]
    public void GetTemplateNode()
    {
        var templateNode = ParentNode.GetTemplateNode();
        templateNode.MessageTemplate.Should().Be("{" + Name + "}");

        var templateNode2 = ParentNode.GetTemplateNode();
        templateNode2.Should().BeSameAs(templateNode);
    }

    [Test]
    public void Create()
    {
        var node = new HoleNode(NodeCache.Instance, Name);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.Name.Should().Be(Name);
    }
}