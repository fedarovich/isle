using System;
using FluentAssertions;
using Isle.Converters;
using NUnit.Framework;

namespace Isle.Core.Tests.Converters;

public class ValueNameConvertersTests
{
    [Test]
    public void WithMemoization()
    {
        var converter = ValueNameConverters.WithMemoization(x => x + x);

        var x = "x";
        var xx1 = converter(x);
        xx1.Should().Be(x + x);
        var xx2 = converter(new string(x.ToCharArray()));
        xx2.Should().BeSameAs(xx1);
    }

    [Test]
    public void WithMemoizationPositiveCacheSize()
    {
        var converter = ValueNameConverters.WithMemoization(x => x + x, 2);

        var x = "x";
        var xx1 = converter(x);
        xx1.Should().Be(x + x);
        var xx2 = converter(new string(x.ToCharArray()));
        xx2.Should().BeSameAs(xx1);

        var y = "y";
        var yy1 = converter(y);
        yy1.Should().Be(y + y);
        var yy2 = converter(new string(y.ToCharArray()));
        yy2.Should().BeSameAs(yy1);

        var z = "z";
        var zz1 = converter(z);
        zz1.Should().Be(z + z);
        var zz2 = converter(new string(z.ToCharArray()));
        zz2.Should().Be(z + z);
        zz2.Should().NotBeSameAs(zz1);
    }

    [Test]
    public void WithMemoizationNegativeCacheSize()
    {
        Action action = () => ValueNameConverters.WithMemoization(x => x + x, -1);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void WithMemoizationZeroCacheSize()
    {
        var converter = (string x) => x + x;
        converter.WithMemoization(0).Should().BeSameAs(converter);
    }

    [Test]
    public void WithMemoizationInt32MaxCacheSize()
    {
        var noLimitConverter = ValueNameConverters.WithMemoization(x => x + x);
        var converter = ValueNameConverters.WithMemoization(x => x + x, int.MaxValue);
        converter.Method.Should().BeSameAs(noLimitConverter.Method);
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
    [TestCase("@ab", ExpectedResult = "@ab")]
    [TestCase(" @ab ", ExpectedResult = "@ab")]
    [TestCase("@ab + @c", ExpectedResult = "@abc")]
    [TestCase("(@ab + @c)", ExpectedResult = "@abc")]
    [TestCase(
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + " +
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + " +
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + " +
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + ",
        ExpectedResult = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890" +
                         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890" +
                         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890" +
                         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890")]
    public string SerilogCompatibleWithoutCapitalization(string expression)
    {
        var converter = ValueNameConverters.SerilogCompatible(false);
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
    [TestCase("@ab", ExpectedResult = "@Ab")]
    [TestCase(" @ab ", ExpectedResult = "@Ab")]
    [TestCase("@ab + @c", ExpectedResult = "@Abc")]
    [TestCase("(@ab + @c)", ExpectedResult = "@Abc")]
    [TestCase(
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + " +
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + " +
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + " +
        "abcdefghijklmnopqrstuvwxyz.ABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890 + ",
        ExpectedResult = "AbcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890" +
                         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890" +
                         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890" +
                         "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_1234567890")]
    public string SerilogCompatibleWithCapitalization(string expression)
    {
        var converter = ValueNameConverters.SerilogCompatible(true);
        return converter(expression);
    }

    [Test]
    public void SerilogCompatibleException(
        [Values(null, "", " ", "+", "()", "\"", "@", "@+")] string expression,
        [Values] bool capitalize)
    {
        var converter = ValueNameConverters.SerilogCompatible(capitalize);
        Action action = () => converter(expression);
        action.Should().Throw<ArgumentException>();
    }

    [TestCase("x", ExpectedResult = "X")]
    [TestCase("X", ExpectedResult = "X")]
    [TestCase("1", ExpectedResult = "1")]
    [TestCase("_", ExpectedResult = "_")]
    [TestCase("xy", ExpectedResult = "Xy")]
    [TestCase("Xy", ExpectedResult = "Xy")]
    [TestCase("1y", ExpectedResult = "1y")]
    [TestCase("_y", ExpectedResult = "_y")]
    [TestCase("@x", ExpectedResult = "@X")]
    [TestCase("@X", ExpectedResult = "@X")]
    [TestCase("@1", ExpectedResult = "@1")]
    [TestCase("@_", ExpectedResult = "@_")]
    [TestCase("@xy", ExpectedResult = "@Xy")]
    [TestCase("@Xy", ExpectedResult = "@Xy")]
    [TestCase("@1y", ExpectedResult = "@1y")]
    [TestCase("@_y", ExpectedResult = "@_y")]
    public string CapitalizeFirstCharacter(string expression)
    {
        var converter = ValueNameConverters.CapitalizeFirstCharacter();
        return converter(expression);
    }

    [Test]
    public void CapitalizeFirstCharacterThrows(
        [Values(null, "", "@")] string expression)
    {
        var converter = ValueNameConverters.CapitalizeFirstCharacter();
        Action action = () => converter(expression);
        action.Should().Throw<ArgumentException>();
    }
}