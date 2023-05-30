using System;
using FluentAssertions;
using Isle.Configuration;
using NUnit.Framework;

namespace Isle.Core.Tests.Configuration;

internal class IsleConfigurationBuilderExtensionsTests
{
    [TearDown]
    public void TearDown()
    {
        IsleConfiguration.Reset();
    }

    [Test]
    public void Configure()
    {
        var valueNameConverter = (string s) => s;
        IsleConfiguration.Configure(builder => builder
            .WithValueRepresentationPolicy(AutoDestructuringValueRepresentationPolicy.Instance)
            .WithNameConverter(valueNameConverter)
            .IsResettable());
#pragma warning disable CS0618
        IsleConfiguration.Current.ValueNameConverter.Should().Be(valueNameConverter);
#pragma warning restore CS0618
        IsleConfiguration.Current.ValueRepresentationPolicy.Should().Be(AutoDestructuringValueRepresentationPolicy.Instance);
    }

    [Test]
    public void WithValueRepresentationPolicyThrowsArgumentNullException()
    {
        var act = () => IsleConfiguration.Configure(b => b.WithValueRepresentationPolicy(null!).IsResettable());
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void WithNameConverterThrowsArgumentNullException()
    {
        var act = () => IsleConfiguration.Configure(b => b.WithNameConverter(null!).IsResettable());
        act.Should().Throw<ArgumentNullException>();
    }
}