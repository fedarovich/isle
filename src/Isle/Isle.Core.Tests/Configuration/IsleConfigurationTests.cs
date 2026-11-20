using System;
using System.Reflection;
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
        IsleConfiguration.Configure(b => b.IsResettable());
        var act = () => IsleConfiguration.Configure(b => b.IsResettable());
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
        IsleConfiguration.Configure(b => b.IsResettable());
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
            builder.IsResettable = true;
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

    [Test]
    public void ResetThrowsWithoutChangingStateIfNotResettable([Values] bool withHook)
    {
        var hook = Substitute.For<IIsleExtensionConfigurationHook>();

        // This test must not touch CoreConfiguration: its static constructor would see the non-resettable
        // configuration and lock the whole test process into the non-resettable mode.
        IsleConfiguration.Configure(builder =>
        {
            builder.IsResettable = false;
            if (withHook)
            {
                builder.RegisterExtensionConfigurationHook(hook);
            }
        });

        try
        {
            var configuration = IsleConfiguration.Current;

            var reset = () => IsleConfiguration.Reset();
            reset.Should().Throw<InvalidOperationException>().WithMessage("*not resettable*");
            hook.DidNotReceive().ResetExtensionConfiguration();
            IsleConfiguration.Current.Should().BeSameAs(configuration);

            var configure = () => IsleConfiguration.Configure(b => b.IsResettable());
            configure.Should().Throw<InvalidOperationException>().WithMessage("*already been configured*");
            IsleConfiguration.Current.Should().BeSameAs(configuration);
        }
        finally
        {
            ForceReset();
        }
    }

    /// <summary>
    /// Clears a non-resettable configuration, which <see cref="IsleConfiguration.Reset"/> refuses to do.
    /// </summary>
    private static void ForceReset()
    {
        const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Static;
        typeof(IsleConfiguration).GetField("_current", flags)!.SetValue(null, null);
        typeof(CoreConfigurationSnapshot).GetField("_current", flags)!.SetValue(null, null);
    }

    [TearDown]
    public void TearDown()
    {
        IsleConfiguration.Reset();
    }
}