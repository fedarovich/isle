using System;
using FluentAssertions;
using Isle.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace Isle.Core.Tests.Configuration;

internal class IsleConfigurationTests
{
    [Test]
    public void ThrowIfActionIsNull()
    {
        var act = () => IsleConfiguration.Configure(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void ThrowIfAlreadyConfigured()
    {
        IsleConfiguration.Configure(_ => { });
        var act = () => IsleConfiguration.Configure(_ => { });
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void ThrowIfNotConfigured()
    {
        var act = () => IsleConfiguration.Current;
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void DefaultConfiguration()
    {
        IsleConfiguration.Configure(_ => {});
        IsleConfiguration.Current.ValueRepresentationPolicy.Should().Be(DefaultValueRepresentationPolicy.Instance);
        IsleConfiguration.Current.PreserveDefaultValueRepresentationForExplicitNames.Should().BeFalse();
    }

    [Test]
    public void ConfigureAndReset()
    {
        var valueNameConverter = (string s) => s;
        var hook = Substitute.For<IIsleExtensionConfigurationHook>();

        IsleConfiguration.Configure(builder =>
        {
            builder.ValueNameConverter = valueNameConverter;
            builder.ValueRepresentationPolicy = AutoDestructuringValueRepresentationPolicy.Instance;
            builder.PreserveDefaultValueRepresentationForExplicitNames = true;
            builder.RegisterExtensionConfigurationHook(hook);
        });
        hook.Received(1).ApplyExtensionConfiguration();
        hook.DidNotReceive().ResetExtensionConfiguration();
        IsleConfiguration.Current.ValueNameConverter.Should().Be(valueNameConverter);
        IsleConfiguration.Current.ValueRepresentationPolicy.Should().Be(AutoDestructuringValueRepresentationPolicy.Instance);
        IsleConfiguration.Current.PreserveDefaultValueRepresentationForExplicitNames.Should().BeTrue();
        hook.ClearReceivedCalls();

        IsleConfiguration.Reset();
        hook.DidNotReceive().ApplyExtensionConfiguration();
        hook.Received(1).ResetExtensionConfiguration();
        hook.ClearReceivedCalls();

        IsleConfiguration.Reset();
        hook.DidNotReceive().ApplyExtensionConfiguration();
        hook.DidNotReceive().ResetExtensionConfiguration();
    }

    [TearDown]
    public void TearDown()
    {
        IsleConfiguration.Reset();
    }
}