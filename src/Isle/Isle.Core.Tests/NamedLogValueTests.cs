using FluentAssertions;
using Isle.Configuration;
using NUnit.Framework;

namespace Isle.Core.Tests;

internal class NamedLogValueTests
{
    [Test]
    public void Create()
    {
        var namedLogValue = new NamedLogValue("TestValue", "TestName");
        namedLogValue.Value.Should().Be("TestValue");
        namedLogValue.Name.Should().Be("TestName");
    }
}