using FluentAssertions;
using Isle.Configuration;
using NUnit.Framework;

namespace Isle.Core.Tests;

internal class NamedLogValueTests
{
    [Test]
    public void Create([Values] ValueRepresentation representation, [Values] NamedLogValue.Flags flags)
    {
        var namedLogValue = new NamedLogValue("TestValue", "TestName", representation, flags);
        namedLogValue.Value.Should().Be("TestValue");
        namedLogValue.Name.Should().Be("TestName");
        namedLogValue.Representation.Should().Be(representation);
        namedLogValue.HasExplicitName.Should().Be(flags.HasFlag(NamedLogValue.Flags.ExplicitName));
    }
}