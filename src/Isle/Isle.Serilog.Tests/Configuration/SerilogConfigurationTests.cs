using FluentAssertions;
using Isle.Configuration;
using Isle.Serilog.Configuration;
using NUnit.Framework;

namespace Isle.Serilog.Tests.Configuration;

internal class SerilogConfigurationTests
{
    [TearDown]
    public void TearDown()
    {
        IsleConfiguration.Reset();
        SerilogConfiguration.Current = null!;
    }

    [Test]
    public void CurrentNotConfigured()
    {
        SerilogConfiguration.Current.Should().Be(SerilogConfiguration.DefaultConfiguration.Value);
    }

    [Test]
    public void CachingIsEnabledInDefaultConfiguration()
    {
        SerilogConfiguration.DefaultConfiguration.Value.EnableMessageTemplateCaching.Should().BeTrue();
    }
}