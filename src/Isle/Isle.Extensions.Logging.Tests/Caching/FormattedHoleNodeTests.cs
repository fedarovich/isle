using FluentAssertions;
using Isle.Extensions.Logging.Caching;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests.Caching;

internal class FormattedHoleNodeTests : NodeTests<FormattedHoleNode>
{
    private const string Name = "Var";
    private const string Format = "F3";
    private const int Alignment = -6;

    protected override FormattedHoleNode CreateParentNode() => NodeCache.Instance.GetOrAddFormattedHoleNode(Name, Alignment, Format);

    [Test]
    public void GetTemplateNode()
    {
        var templateNode = ParentNode.GetTemplateNode();
        templateNode.MessageTemplate.Should().Be($"{{{Name},{Alignment}:{Format}}}");

        var templateNode2 = ParentNode.GetTemplateNode();
        templateNode2.Should().BeSameAs(templateNode);
    }

    [Test]
    public void Create()
    {
        var node = new FormattedHoleNode(NodeCache.Instance, Name, Alignment, Format);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.Name.Should().Be(Name);
        node.Alignment.Should().Be(Alignment);
        node.Format.Should().Be(Format);
    }
}