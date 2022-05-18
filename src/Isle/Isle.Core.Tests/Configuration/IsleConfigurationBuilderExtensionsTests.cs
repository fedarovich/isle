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
            .WithNameConverter(valueNameConverter));
        IsleConfiguration.Current.ValueNameConverter.Should().Be(valueNameConverter);
        IsleConfiguration.Current.ValueRepresentationPolicy.Should().Be(AutoDestructuringValueRepresentationPolicy.Instance);
    }

    [Test]
    public void WithValueRepresentationPolicyThrowsArgumentNullException()
    {
        var act = () => IsleConfiguration.Configure(b => b.WithValueRepresentationPolicy(null!));
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void WithNameConverterThrowsArgumentNullException()
    {
        var act = () => IsleConfiguration.Configure(b => b.WithNameConverter(null!));
        act.Should().Throw<ArgumentNullException>();
    }
}