using System;
using FluentAssertions;
using Isle.Converters;
using NUnit.Framework;

namespace Isle.Core.Tests.Converters;

public class ValueNameConvertersTests
{
    [Test]
    public void CreateCachingConverter()
    {
        var converter = ValueNameConverters.CreateCachingConverter(x => x + x);

        var x = "x";
        var xx1 = converter(x);
        xx1.Should().Be(x + x);
        var xx2 = converter(new string(x));
        xx2.Should().BeSameAs(xx1);
    }

    [Test]
    public void CreateCachingConverterWithMaxCacheSize()
    {
        var converter = ValueNameConverters.CreateCachingConverter(x => x + x, 2);

        var x = "x";
        var xx1 = converter(x);
        xx1.Should().Be(x + x);
        var xx2 = converter(new string(x));
        xx2.Should().BeSameAs(xx1);

        var y = "y";
        var yy1 = converter(y);
        yy1.Should().Be(y + y);
        var yy2 = converter(new string(y));
        yy2.Should().BeSameAs(yy1);

        var z = "z";
        var zz1 = converter(z);
        zz1.Should().Be(z + z);
        var zz2 = converter(new string(z));
        zz2.Should().Be(z + z);
        zz2.Should().NotBeSameAs(zz1);
    }

    [TestCase("x", ExpectedResult = "x")]
    [TestCase("x ", ExpectedResult = "x")]
    [TestCase(" x", ExpectedResult = "x")]
    [TestCase("x y", ExpectedResult = "xy")]
    [TestCase(" x y ", ExpectedResult = "xy")]
    [TestCase("1", ExpectedResult = "1")]
    [TestCase("1+2", ExpectedResult = "12")]
    [TestCase(" 1 + 2 ", ExpectedResult = "12")]
    [TestCase("_", ExpectedResult = "_")]
    [TestCase(" _ ", ExpectedResult = "_")]
    [TestCase("a.b", ExpectedResult = "ab")]
    [TestCase("a.b()", ExpectedResult = "ab")]
    [TestCase("a[b]", ExpectedResult = "ab")]
    [TestCase(" ab(cd, ef)  * (gh[ij] - \n_0kl)  ", ExpectedResult = "abcdefghij_0kl")]
    public string CreateSerilogCompatibleConverterWithoutCapitalization(string expression)
    {
        var converter = ValueNameConverters.CreateSerilogCompatibleConverter(false, 0);
        return converter(expression);
    }

    [TestCase("x", ExpectedResult = "X")]
    [TestCase("x ", ExpectedResult = "X")]
    [TestCase(" x", ExpectedResult = "X")]
    [TestCase("x y", ExpectedResult = "Xy")]
    [TestCase(" x y ", ExpectedResult = "Xy")]
    [TestCase("1", ExpectedResult = "1")]
    [TestCase("1+2", ExpectedResult = "12")]
    [TestCase(" 1 + 2 ", ExpectedResult = "12")]
    [TestCase("_", ExpectedResult = "_")]
    [TestCase(" _ ", ExpectedResult = "_")]
    [TestCase("a.b", ExpectedResult = "Ab")]
    [TestCase("a.b()", ExpectedResult = "Ab")]
    [TestCase("a[b]", ExpectedResult = "Ab")]
    [TestCase(" ab(cd, ef)  * (gh[ij] - \n_0kl)  ", ExpectedResult = "Abcdefghij_0kl")]
    public string CreateSerilogCompatibleConverterWithCapitalization(string expression)
    {
        var converter = ValueNameConverters.CreateSerilogCompatibleConverter(true, 0);
        return converter(expression);
    }

    [Test]
    public void CreateSerilogCompatibleConvertereException(
        [Values(null, "", " ", "+", "()", "\"")] string expression,
        [Values] bool capitalize)
    {
        var converter = ValueNameConverters.CreateSerilogCompatibleConverter(capitalize, 0);
        Action action = () => converter(expression);
        action.Should().Throw<ArgumentException>();
    }
}