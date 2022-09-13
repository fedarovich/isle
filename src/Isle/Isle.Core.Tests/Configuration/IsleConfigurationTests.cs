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
        IsleConfiguration.Current.CacheLiteralValues.Should().BeFalse();
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
            builder.CacheLiteralValues = true;
            builder.RegisterExtensionConfigurationHook(hook);
        });
        hook.Received(1).ApplyExtensionConfiguration();
        hook.DidNotReceive().ResetExtensionConfiguration();
#pragma warning disable CS0618
        IsleConfiguration.Current.ValueNameConverter.Should().Be(valueNameConverter);
#pragma warning restore CS0618
        IsleConfiguration.Current.ValueRepresentationPolicy.Should().Be(AutoDestructuringValueRepresentationPolicy.Instance);
        IsleConfiguration.Current.PreserveDefaultValueRepresentationForExplicitNames.Should().BeTrue();
        IsleConfiguration.Current.CacheLiteralValues.Should().BeTrue();
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