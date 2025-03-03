using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using VerifyCapturedNamesCS = Isle.Converters.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    Isle.Converters.Roslyn.Analyzers.CapturedNamesAnalyzer,
    Isle.Converters.Roslyn.Analyzers.CapturedNameMustBeValidIdentifierCodeFixProvider>;
using VerifyExplicitNamesCS = Isle.Converters.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    Isle.Converters.Roslyn.Analyzers.CapturedNamesAnalyzer,
    Isle.Converters.Roslyn.Analyzers.ExplicitNameShouldNotHaveWhiteSpacesCodeFixProvider>;

namespace Isle.Converters.Roslyn.Analyzers.Test;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CapturedNamesAnalyzerMELTests
{
    public static LogLevel[] LogLevels =
    [
        LogLevel.Trace,
        LogLevel.Debug,
        LogLevel.Information,
        LogLevel.Warning,
        LogLevel.Error,
        LogLevel.Critical
    ];

    public const string Usings =
        """
        using System;
        using System.Collections.Generic;
        using Microsoft.Extensions.Logging;
        using Isle;
        using Isle.Extensions;
        using Isle.Extensions.Logging;
        """;

    public const string SharedCode =
        """
            public class Value
            {
                public int IntProperty { get; set; }
            
                public string StringProperty { get; set; }
                
                public string Method() => "Test";
                
                public int Method2(int i) => i;
            }
            
            public record Rec(int Value);
        """;

    #region No Diagnostic
    
    #region Capture

    public static string[] ValidExpressions =
    [
        "{value}",
        "{@value}",
        "{value!}",
        "{value.IntProperty}",
        "{value?.IntProperty}",
        "{value.IntProperty++}",
        "{value.IntProperty--}",
        "{++value.IntProperty}",
        "{--value.IntProperty}",
        "{value.Method()}",
        "{value?.Method()}",
        "{value.Method2(5)}",
        "{value?.Method2(5)}",
        "{(value)}",
        "{(short) value.IntProperty}",
        "{checked((short) value.IntProperty)}",
        "{unchecked((short) value.IntProperty)}",
        "{value.Named(\"Val\")}",
        "{value.Named(\"@Val\")}",
        "{value.Named(\"$Val\")}",
        "{value.IntProperty.Named(\"Int\")}",
        "{value.IntProperty.Named(\"@Int\")}",
        "{value.IntProperty.Named(\"$Int\")}",
        "{(value?.IntProperty).Named(\"Int\")}",
        "{(value?.IntProperty).Named(\"@Int\")}",
        "{(value?.IntProperty).Named(\"$Int\")}",
        "{new LiteralValue(\"Literal\")}"
    ];

    [Test]
    public async Task Log_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}($"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogEventId_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}(42, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogException_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(ex, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogEventIdException_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(42, ex, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevel_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevelEventId_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, 42, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevelException_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, ex, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevelEventIdException_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, 42, ex, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task BeginScopeInterpolated_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(ValidExpressions))] string expression)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        using (logger.BeginScopeInterpolated($"Test {{expression}}"))
                        {
                        }
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    #endregion

    #region Named

    public static string[] ValidNames =
    [
        "_value_1",
        "@_value_1",
        "$_value_1",
        "value_1",
        "@value_1",
        "$value_1",
        "Value_1",
        "@Value_1",
        "$Value_1"
    ];

    [Test]
    public async Task Log_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y)
                    {
                        logger.Log{{level}}($"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogEventId_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y)
                    {
                        logger.Log{{level}}(42, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogException_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y, Exception ex)
                    {
                        logger.Log{{level}}(ex, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogEventIdException_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y, Exception ex)
                    {
                        logger.Log{{level}}(42, ex, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevel_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y)
                    {
                        logger.Log(LogLevel.{{level}}, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevelEventId_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y)
                    {
                        logger.Log(LogLevel.{{level}}, 42, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevelException_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, ex, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogLevelEventIdException_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, 42, ex, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task BeginScopeInterpolated_WithValidName_NoDiagnostics(
        [ValueSource(nameof(ValidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, int x, int y, Exception ex)
                    {
                        using (logger.BeginScopeInterpolated($"Test {(x + y).Named("{{name}}")}."))
                        {
                        }
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    #endregion

    #endregion

    #region ISLE4000: CapturedNameMustBeValidIdentifierDiagnosticId

    public static string[] InvalidExpressions =
    [
        "42",
        "true",
        "\"Test\"",
        "typeof(string)",
        "nameof(x)",
        "default(int)",
        "sizeof(int)",
        "new Value()",
        "new {x, y}",
        "new []{x, y}",
        "(x, y)",
        "x + y",
        "(x == y)",
        "(s ??= string.Empty)",
        "(s ?? string.Empty)",
        "list[0]",
        "list?[0]",
        "(b ? x : y)",
        "obj as string",
        "obj is string",
        "obj is string str",
        "^x",
        "$\"Interpolated String {x}\"",
        "rec with { Value = y }",
        "x switch { 0 => 42, 1 => 128, _ => 1000 }"
    ];

    [Test]
    public async Task Log_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log{{{level}}}($"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log{{{level}}}($"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogEventId_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log{{{level}}}(42, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log{{{level}}}(42, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogException_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log{{{level}}}(ex, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log{{{level}}}(ex, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogEventIdException_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log{{{level}}}(42, ex, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log{{{level}}}(42, ex, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevel_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log(LogLevel.{{{level}}}, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log(LogLevel.{{{level}}}, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevelEventId_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log(LogLevel.{{{level}}}, 42, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec)
                    {
                        logger.Log(LogLevel.{{{level}}}, 42, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevelException_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log(LogLevel.{{{level}}}, ex, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log(LogLevel.{{{level}}}, ex, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevelEventIdException_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log(LogLevel.{{{level}}}, 42, ex, $"Test {{|#0:{{{expression}}}|}} and {x}.");
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        logger.Log(LogLevel.{{{level}}}, 42, ex, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task BeginScopeInterpolated_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(InvalidExpressions))] string expression)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        using (logger.BeginScopeInterpolated($"Test {{|#0:{{{expression}}}|}} and {x}."))
                        {
                        }
                    }
                }
            }
            """;

        var trimmedExpression = expression != "(x, y)" && expression.StartsWith('(') && expression.EndsWith(')')
            ? expression[1..^1]
            : expression;
        var fix =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, bool b, int x, int y, string s, List<int> list, object obj, Rec rec, Exception ex)
                    {
                        using (logger.BeginScopeInterpolated($"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}."))
                        {
                        }
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    #endregion

    #region ISLE4001: CapturedNameMustBeUniqueDiagnosticId

    public static (string, string, string)[] NotUniqueNames =
    [
        ("value", "value", "value"),
        ("value", "Value", "VALUE"),
        ("value", "@value", "$value"),
        ("value", "@Value", "$VALUE")
    ];

    [Test]
    public async Task Log_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z)
                    {
                        logger.Log{{{level}}}(
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogEventId_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z)
                    {
                        logger.Log{{{level}}}(42, 
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogException_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                    {
                        logger.Log{{{level}}}(ex, 
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogEventIdException_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                    {
                        logger.Log{{{level}}}(42, ex, 
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevel_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z)
                    {
                        logger.Log(LogLevel.{{{level}}},
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevelEventId_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z)
                    {
                        logger.Log(LogLevel.{{{level}}}, 42, 
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevelException_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                    {
                        logger.Log(LogLevel.{{{level}}}, ex, 
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevelEventIdException_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                    {
                        logger.Log(LogLevel.{{{level}}}, 42, ex, 
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}.");
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task BeginScopeInterpolated_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(NotUniqueNames))] (string, string, string) names)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
                
                class TestClass
                {
                    public int Value { get; set; }
                
                    public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                    {
                        using (logger.BeginScopeInterpolated(
                            $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:x.Named("{{{names.Item1}}}")|}}, {{|#3:y.Named("{{{names.Item2}}}")|}}, {{|#4:z.Named("{{{names.Item3}}}")|}}."))
                        {
                        }
                    }
                }
            }

            """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments(names.Item1.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(3).WithArguments(names.Item2.TrimStart('$', '@')),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(4).WithArguments(names.Item3.TrimStart('$', '@'))
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    #endregion

    #region ISLE4002: ExplicitNameShouldBeConstantDiagnosticId

    [Test]
    public async Task LogNamed_WithNotConstantName_ExplicitNameShouldBeConstantDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level)
    {
        var test =
            $$$"""
            {{{Usings}}}

            namespace Test
            {
                {{{SharedCode}}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, string name)
                    {
                        logger.Log{{{level}}}($"Test {value.Named({|#0:name|})}, {value.Named({|#1:name|})}.");
                    }
                }
            }
            """;

        DiagnosticResult[] expected =
        [
            VerifyExplicitNamesCS.Diagnostic("ISLE4002").WithLocation(0),
            VerifyExplicitNamesCS.Diagnostic("ISLE4002").WithLocation(1)
        ];
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    #endregion

    #region ISLE4003: ExplicitNameMustBeValidIdentifierDiagnosticId

    public static readonly string[] InvalidNames =
    [
        "",
        " ",
        "1",
        "1a",
        "@1",
        "$1",
        "a.b",
        "(a)",
        "a b",
        "@ a",
        "$ a",
        "$@a",
        "@$a",
        "@@a"
    ];

    [Test]
    public async Task Log_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}($"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogEventId_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}(42, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogException_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogEventIdException_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(42, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevel_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevelEventId_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, 42, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevelException_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogLevelEventIdException_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, 42, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task BeginScope_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(InvalidNames))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        using (logger.BeginScopeInterpolated($"Test {value.Named({|#0:"{{name}}"|})}."))
                        {
                        }
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    #endregion

    #region ISLE4004: ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId

    public static readonly string[] NamesWithWhiteSpaces =
    [
        " Value",
        "Value ",
        " Value ",
        " @Value",
        "@Value ",
        " @Value ",
        " $Value",
        "$Value ",
        " $Value ",
    ];

    [Test]
    public async Task Log_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}($"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix = 
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}($"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogEventId_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}(42, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log{{level}}(42, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogException_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(ex, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogEventIdException_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(42, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log{{level}}(42, ex, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevel_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevelEventId_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, 42, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        logger.Log(LogLevel.{{level}}, 42, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevelException_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, ex, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogLevelEventIdException_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, 42, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value, Exception ex)
                    {
                        logger.Log(LogLevel.{{level}}, 42, ex, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task BeginScope_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogLevel level,
        [ValueSource(nameof(NamesWithWhiteSpaces))] string name)
    {
        var test =
            $$"""
              {{Usings}}

              namespace Test
              {
                  {{SharedCode}}
              
                  class TestClass
                  {
                      public static void TestLog(ILogger logger, Value value)
                      {
                          using (logger.BeginScopeInterpolated($"Test {value.Named({|#0:"{{name}}"|})}."))
                          {
                          }
                      }
                  }
              }
              """;

        var fix =
            $$"""
            {{Usings}}

            namespace Test
            {
                {{SharedCode}}
            
                class TestClass
                {
                    public static void TestLog(ILogger logger, Value value)
                    {
                        using (logger.BeginScopeInterpolated($"Test {value.Named("{{name.Trim()}}")}."))
                        {
                        }
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    #endregion
}

file static class Extensions
{
    internal static string TrimOperator(this string name)
    {
        return name.StartsWith('@') || name.StartsWith('$') ? name.Substring(1) : name;
    }
}