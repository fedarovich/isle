using FluentAssertions;
using Isle.Configuration;
using Isle.Extensions.Logging.Configuration;
using NUnit.Framework;

namespace Isle.Extensions.Logging.Tests.Configuration;

public class ExtensionsLoggingConfigurationTests
{
    [TearDown]
    public void TearDown()
    {
        IsleConfiguration.Reset();
        ExtensionsLoggingConfiguration.Current = null!;
    }

    [Test]
    public void CurrentNotConfigured()
    {
        ExtensionsLoggingConfiguration.Current.Should().Be(ExtensionsLoggingConfiguration.DefaultConfiguration.Value);
    }

    [Test]
    public void CachingIsEnabledInDefaultConfiguration()
    {
        ExtensionsLoggingConfiguration.DefaultConfiguration.Value.EnableMessageTemplateCaching.Should().BeTrue();
    }
}