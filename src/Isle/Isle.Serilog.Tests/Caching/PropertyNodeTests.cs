using FluentAssertions;
using Isle.Serilog.Caching;
using NUnit.Framework;
using Serilog.Parsing;

namespace Isle.Serilog.Tests.Caching;

internal class PropertyNodeTests : NodeTests<PropertyNode>
{
    private const string Name = "Var";
    private const string Format = "F3";
    private const int Alignment = -6;

    protected override PropertyNode CreateParentNode() => NodeCache.Instance.GetOrAddPropertyNode(Name, Name, Alignment, Format);

    [Test]
    public void GetTemplateNode()
    {
        var templateNode = ParentNode.GetTemplateNode();
        templateNode.MessageTemplate.Text.Should().Be($"{{{Name},{Alignment}:{Format}}}");

        var templateNode2 = ParentNode.GetTemplateNode();
        templateNode2.Should().BeSameAs(templateNode);
    }

    [Test]
    public void Create()
    {
        var node = new PropertyNode(NodeCache.Instance, Name, Name, Alignment, Format);
        node.Parent.Should().Be(NodeCache.Instance);
        node.Depth.Should().Be(1);
        node.RawText.Should().Be($"{{{Name},{Alignment}:{Format}}}");
        node.Token.PropertyName.Should().Be(Name);
        node.Token.Format.Should().Be(Format);
        node.Token.Alignment.Should().Be(new Alignment(AlignmentDirection.Left, -Alignment));
        node.Token.Destructuring.Should().Be(Destructuring.Default);
    }
}