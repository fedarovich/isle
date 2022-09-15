using System;
using FluentAssertions;
using Isle.Configuration;
using Isle.Extensions;
using NUnit.Framework;

namespace Isle.Core.Tests.Extensions;

public class LoggingExtensionsTests
{
    private static readonly DateTime TestDateTime = DateTime.Now;
    private static readonly TimeSpan TestTimeSpan = TestDateTime.TimeOfDay;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        IsleConfiguration.Configure(builder => builder
            .WithValueRepresentationPolicy(new TestValueRepresentationPolicy())
            .WithNameConverter(n => n.ToUpperInvariant()));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        IsleConfiguration.Reset();
    }

    [Test]
    public void NamedWithNullNameThrowsArgumentException()
    {
        Action act = () => 0.Named(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NamedWithEmptyNameThrowsArgumentException()
    {
        Action act = () => 0.Named(string.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NamedWithWhitespaceNameThrowsArgumentException()
    {
        Action act = () => 0.Named(" ");
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NamedWithDefaultValueRepresentation([Values("x", "$x", "@x")] string name)
    {
        var named = 5.Named(name);
        named.Name.Should().Be(name);
        named.Value.Should().Be(5);
    }

    [Test]
    public void NamedWithDestructureValueRepresentation([Values("x", "$x", "@x")] string name)
    {
        var named = TestTimeSpan.Named(name);
        named.Name.Should().Be(name == "x" ? $"@{name}" : name);
        named.Value.Should().Be(TestTimeSpan);
    }

    [Test]
    public void NamedWithStringifyValueRepresentation([Values("x", "$x", "@x")] string name)
    {
        var named = TestDateTime.Named(name);
        named.Name.Should().Be(name == "x" ? $"${name}" : name);
        named.Value.Should().Be(TestDateTime);
    }

    [Test]
    public void GetNameFromCallerArgumentExpressionWithDefaultValueRepresentation([Values("x", "$x", "@x")] string name)
    {
        var newName = name.GetNameFromCallerArgumentExpression<int>(IsleConfiguration.Current);
        newName.Should().Be(name.ToUpperInvariant());
    }

    [Test]
    public void GetNameFromCallerArgumentExpressionWithDestructureValueRepresentation([Values("x", "$x", "@x")] string name)
    {
        var newName = name.GetNameFromCallerArgumentExpression<TimeSpan>(IsleConfiguration.Current);
        newName.Should().Be(name == "x" ? "@X" : name.ToUpperInvariant());
    }

    [Test]
    public void GetNameFromCallerArgumentExpressionWithStringifyValueRepresentation([Values("x", "$x", "@x")] string name)
    {
        var newName = name.GetNameFromCallerArgumentExpression<DateTime>(IsleConfiguration.Current);
        newName.Should().Be(name == "x" ? "$X" : name.ToUpperInvariant());
    }

    private class TestValueRepresentationPolicy : IValueRepresentationPolicy
    {
        public ValueRepresentation GetRepresentationOfType<T>()
        {
            if (typeof(T) == typeof(DateTime))
                return ValueRepresentation.Stringify;
            if (typeof(T) == typeof(TimeSpan))
                return ValueRepresentation.Destructure;
            return ValueRepresentation.Default;
        }
    }
}