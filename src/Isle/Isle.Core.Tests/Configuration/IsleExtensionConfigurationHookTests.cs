using FluentAssertions;
using Isle.Configuration;
using NUnit.Framework;

namespace Isle.Core.Tests.Configuration;

internal class IsleExtensionConfigurationHookTests
{
    private class Impl : IIsleExtensionConfigurationHook
    {
        public void ApplyExtensionConfiguration()
        {
        }
    }

    [Test]
    public void ResetExtensionConfigurationDefaultImplDoesNotThrow()
    {
        IIsleExtensionConfigurationHook impl = new Impl();
        var act = () => impl.ResetExtensionConfiguration();
        act.Should().NotThrow();
    }
}