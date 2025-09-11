using FluentAssertions;
using Isle.Configuration;

namespace Isle.Converters.Roslyn.Tests;

public class RoslynNameConverterTests
{
    private static string Fallback(string name) => $"FALLBACK_{name}";

    private static RoslynNameConverter CreateConverter(bool capitalize, params string[] prefixesToRemove)
    {
        var builder = new RoslynNameConverterConfigurationBuilder();
        builder.WithFallback(Fallback);

        if (prefixesToRemove.Length > 0)
        {
            builder.RemoveMethodPrefixes(prefixesToRemove);
        }

        if (capitalize)
        {
            builder.CapitalizeFirstCharacter();
        }

        return builder.Build();
    }

    #region Simple expressions

    private static readonly object[][] SimpleCases =
    [
        ["test", "test", false],
        ["@test", "@test", false],
        ["Test", "Test", false],
        ["@Test", "@Test", false],
        ["test", "Test", true],
        ["@test", "@Test", true],
        ["Test", "Test", true],
        ["@Test", "@Test", true],
    ];

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Identifier(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"{member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void NotNull(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"{member}!");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void PreIncrement(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"++{member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void PreDecrement(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"--{member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void PostIncrement(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"{member}++");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void PostDecrement(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"{member}--");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Property(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"x.{member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void ConditionalProperty(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"x?.{member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Function(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"{member}()");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Method(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"x.{member}()");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void ConditionalMethod(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"x?.{member}()");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Parenthesized(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"({member})");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void PointerIndirection(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"*{member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Cast(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"(string) {member}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Checked(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"checked((int) {member})");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(SimpleCases))]
    public void Unchecked(string member, string expectedName, bool capitalize)
    {
        var converter = CreateConverter(capitalize);

        var name = converter.Convert($"unchecked((int) {member})");

        name.Should().Be(expectedName);
    }

    #endregion

    #region Method Prefix Removal

    private static readonly object[][] MethodPrefixRemovalCases =
    [
        ["isTest()", "Test", false, "is"],
        ["@isTest()", "@Test", false, "is"],
        ["istest()", "test", false, "is"],
        ["@istest()", "@test", false, "is"],
        ["isTest", "isTest", false, "is"],
        ["@isTest", "@isTest", false, "is"],

        ["isTest()", "Test", true, "is"],
        ["@isTest()", "@Test", true, "is"],
        ["istest()", "Test", true, "is"],
        ["@istest()", "@Test", true, "is"],
        ["isTest", "IsTest", true, "is"],
        ["@isTest", "@IsTest", true, "is"],
    ];

    [Test]
    [TestCaseSource(nameof(MethodPrefixRemovalCases))]
    public void FunctionPrefixRemoval(string expression, string expectedName, bool capitalize, string prefix)
    {
        var converter = CreateConverter(capitalize, prefix);

        var name = converter.Convert($"{expression}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(MethodPrefixRemovalCases))]
    public void MethodPrefixRemoval(string expression, string expectedName, bool capitalize, string prefix)
    {
        var converter = CreateConverter(capitalize, prefix);

        var name = converter.Convert($"x.{expression}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(MethodPrefixRemovalCases))]
    public void ConditionalMethodPrefixRemoval(string expression, string expectedName, bool capitalize, string prefix)
    {
        var converter = CreateConverter(capitalize, prefix);

        var name = converter.Convert($"x?.{expression}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(MethodPrefixRemovalCases))]
    public void ConditionalMethodPrefixRemoval2(string expression, string expectedName, bool capitalize, string prefix)
    {
        var converter = CreateConverter(capitalize, prefix);

        var name = converter.Convert($"x?.y.{expression}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(MethodPrefixRemovalCases))]
    public void ConditionalMethodPrefixRemoval3(string expression, string expectedName, bool capitalize, string prefix)
    {
        var converter = CreateConverter(capitalize, prefix);

        var name = converter.Convert($"x.y?.{expression}");

        name.Should().Be(expectedName);
    }

    [Test]
    [TestCaseSource(nameof(MethodPrefixRemovalCases))]
    public void ConditionalMethodPrefixRemoval4(string expression, string expectedName, bool capitalize, string prefix)
    {
        var converter = CreateConverter(capitalize, prefix);

        var name = converter.Convert($"x?.y?.{expression}");

        name.Should().Be(expectedName);
    }

    #endregion

    #region Complex Expressions

    [Test]
    public void ComplexPropertyExpression()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(a + b)[x]?.M().P?.X[0]?.Y()[Z].@Test");

        name.Should().Be("@Test");
    }

    [Test]
    public void ComplexConditionalPropertyExpression()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(a + b)[x]?.M().P?.X[0]?.Y()[Z]?.@Test");

        name.Should().Be("@Test");
    }

    [Test]
    public void ComplexMethodExpression()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(a + b)[x]?.M().P?.X[0]?.Y()[Z].@Test()");

        name.Should().Be("@Test");
    }

    [Test]
    public void ComplexConditionalMethodExpression()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(a + b)[x]?.M().P?.X[0]?.Y()[Z]?.@Test()");

        name.Should().Be("@Test");
    }

    #endregion

    #region Unsupported expressions

    [Test]
    public void NumericLiteral()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("42");

        name.Should().Be("FALLBACK_42");
    }

    [Test]
    public void StringLiteral()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("\"test\"");

        name.Should().Be("FALLBACK_\"test\"");
    }

    [Test]
    public void BooleanLiteral()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("true");

        name.Should().Be("FALLBACK_true");
    }

    [Test]
    public void NullLiteral()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("null");

        name.Should().Be("FALLBACK_null");
    }

    [Test]
    public void TypeOf()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("typeof(string)");

        name.Should().Be("FALLBACK_typeof(string)");
    }

    [Test]
    public void NameOf()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("nameof(test)");

        name.Should().Be("FALLBACK_nameof(test)");
    }

    [Test]
    public void AddressOf()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("&test");

        name.Should().Be("FALLBACK_&test");
    }

    [Test]
    public void DefaultOf()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("default(int)");

        name.Should().Be("FALLBACK_default(int)");
    }

    [Test]
    public void SizeOf()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("sizeof(int)");

        name.Should().Be("FALLBACK_sizeof(int)");
    }

    [Test]
    public void New()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("new object()");

        name.Should().Be("FALLBACK_new object()");
    }

    [Test]
    public void NewArray()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("new [] {x, y}");

        name.Should().Be("FALLBACK_new [] {x, y}");
    }
    
    [Test]
    public void Tuple()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(x, y)");

        name.Should().Be("FALLBACK_(x, y)");
    }

    [Test]
    public void Collection()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("[x, y]");

        name.Should().Be("FALLBACK_[x, y]");
    }

    [Test]
    public void Anonymous()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("new {x, y}");

        name.Should().Be("FALLBACK_new {x, y}");
    }

    [Test]
    public void Range()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("[x..y]");

        name.Should().Be("FALLBACK_[x..y]");
    }

    [Test]
    public void Arithmetic()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x + y");

        name.Should().Be("FALLBACK_x + y");
    }

    [Test]
    public void Assignment()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(x = y)");

        name.Should().Be("FALLBACK_(x = y)");
    }

    [Test]
    public void ConditionalAssignment()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("(x ??= y)");

        name.Should().Be("FALLBACK_(x ??= y)");
    }

    [Test]
    public void Conditional()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x ?? y");

        name.Should().Be("FALLBACK_x ?? y");
    }

    [Test]
    public void Indexer()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x[y]");

        name.Should().Be("FALLBACK_x[y]");
    }

    [Test]
    public void ConditionalIndexer()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x?[y]");

        name.Should().Be("FALLBACK_x?[y]");
    }

    [Test]
    public void TernaryOperator()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x ? y : z");

        name.Should().Be("FALLBACK_x ? y : z");
    }

    [Test]
    public void As()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x as String");

        name.Should().Be("FALLBACK_x as String");
    }

    [Test]
    public void Is()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x is String");

        name.Should().Be("FALLBACK_x is String");
    }

    [Test]
    public void IsPattern()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x is String s");

        name.Should().Be("FALLBACK_x is String s");
    }

    [Test]
    public void IndexFromEnd()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("^x");

        name.Should().Be("FALLBACK_^x");
    }

    [Test]
    public void With()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("x with { A = a }");

        name.Should().Be("FALLBACK_x with { A = a }");
    }

    [Test]
    public void Ref()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("ref x");

        name.Should().Be("FALLBACK_ref x");
    }

    [Test]
    public void InterpolatedString()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert(
            """
            $"Interolated string {value}."
            """);

        name.Should().Be(
            """
            FALLBACK_$"Interolated string {value}."
            """);
    }

    [Test]
    public void Switch()
    {
        var converter = CreateConverter(false);

        var name = converter.Convert("""
                                     x switch
                                     {
                                        0 => a,
                                        1 => b,
                                        _ => c
                                     }
                                     """);

        name.Should().Be(
            """
            FALLBACK_x switch
            {
               0 => a,
               1 => b,
               _ => c
            }
            """);
    }

    #endregion
}
