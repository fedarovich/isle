using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using Serilog.Events;
using VerifyCapturedNamesCS = Isle.Converters.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    Isle.Converters.Roslyn.Analyzers.CapturedNamesAnalyzer,
    Isle.Converters.Roslyn.Analyzers.CapturedNameMustBeValidIdentifierCodeFixProvider>;
using VerifyExplicitNamesCS = Isle.Converters.Roslyn.Analyzers.Test.CSharpCodeFixVerifier<
    Isle.Converters.Roslyn.Analyzers.CapturedNamesAnalyzer,
    Isle.Converters.Roslyn.Analyzers.ExplicitNameShouldNotHaveWhiteSpacesCodeFixProvider>;

namespace Isle.Converters.Roslyn.Analyzers.Test;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CapturedNamesAnalyzerSerilogTests
{
    public static LogEventLevel[] LogLevels =
    [
        LogEventLevel.Verbose,
        LogEventLevel.Debug,
        LogEventLevel.Information,
        LogEventLevel.Warning,
        LogEventLevel.Error,
        LogEventLevel.Fatal
    ];

    public const string Usings =
        """
        using System;
        using System.Collections.Generic;
        using Serilog;
        using Serilog.Events;
        using Isle;
        using Isle.Extensions;
        using Isle.Serilog;
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated($"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogException_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated(ex, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task WriteInterpolated_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, $"Test {{expression}}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task WriteInterpolatedException_WithValidIdentifier_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, ex, $"Test {{expression}}.");
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated($"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task LogException_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated(ex, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task WriteInterpolated_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, $"Test {(x + y).Named("{{name}}")}.");
                    }
                }
            }
            """;

        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test);
    }

    [Test]
    public async Task WriteInterpolatedException_WithValidName_NoDiagnostics(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, ex, $"Test {(x + y).Named("{{name}}")}.");
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{{level}}}Interpolated($"Test {{|#0:{{{expression}}}|}} and {x}.");
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
                        logger.{{{level}}}Interpolated($"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogException_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{{level}}}Interpolated(ex, $"Test {{|#0:{{{expression}}}|}} and {x}.");
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
                        logger.{{{level}}}Interpolated(ex, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task WriteInterpolated_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{{level}}}, $"Test {{|#0:{{{expression}}}|}} and {x}.");
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
                        logger.WriteInterpolated(LogEventLevel.{{{level}}}, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
                    }
                }
            }
            """;

        var expected = VerifyCapturedNamesCS.Diagnostic("ISLE4000").WithLocation(0).WithArguments(expression);
        await VerifyCapturedNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task WriteInterpolatedException_WithInvalidIdentifier_CapturedNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{{level}}}, ex, $"Test {{|#0:{{{expression}}}|}} and {x}.");
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
                        logger.WriteInterpolated(LogEventLevel.{{{level}}}, ex, $"Test {({{{trimmedExpression}}}).Named("TODO_SPECIFY_NAME")} and {x}.");
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{{level}}}Interpolated(
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{{level}}}Interpolated(ex, 
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
    public async Task WriteInterpolated_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{{level}}},
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
    public async Task WriteInterpolatedException_WithNotUniqueIdentifiers_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{{level}}}, ex, 
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
    public async Task Log_WithMatchingPropertyAndMethod_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level)
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
                       
                       public int GetValue() => 0;
                   
                       public void TestLog(ILogger logger, int value, int x, int y, int z)
                       {
                           logger.{{{level}}}Interpolated(
                               $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:GetValue()|}}.");
                       }
                   }
               }

               """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments("Value")
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogException_WithMatchingPropertyAndMethod_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level)
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
                       
                       public int GetValue() => 0;
                   
                       public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                       {
                           logger.{{{level}}}Interpolated(ex,
                               $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:GetValue()|}}.");
                       }
                   }
               }

               """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments("Value")
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task WriteInterpolated_WithMatchingPropertyAndMethod_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level)
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
                       
                       public int GetValue() => 0;
                   
                       public void TestLog(ILogger logger, int value, int x, int y, int z)
                       {
                           logger.WriteInterpolated(LogEventLevel.{{{level}}},
                               $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:GetValue()|}}.");
                       }
                   }
               }

               """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments("Value")
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task WriteInterpolatedException_WithMatchingPropertyAndMethod_CapturedNameMustBeUniqueDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level)
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
                       
                       public int GetValue() => 0;
                   
                       public void TestLog(ILogger logger, int value, int x, int y, int z, Exception ex)
                       {
                           logger.WriteInterpolated(LogEventLevel.{{{level}}},ex,
                               $"Test {{|#0:value|}}, {{|#1:Value|}}, {{|#2:GetValue()|}}.");
                       }
                   }
               }

               """;

        DiagnosticResult[] expected = [
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(1).WithArguments("Value"),
            VerifyCapturedNamesCS.Diagnostic("ISLE4001").WithLocation(2).WithArguments("Value")
        ];
        await VerifyCapturedNamesCS.VerifyAnalyzerAsync(test, expected);
    }
    
    #endregion

    #region ISLE4002: ExplicitNameShouldBeConstantDiagnosticId

    [Test]
    public async Task LogNamed_WithNotConstantName_ExplicitNameShouldBeConstantDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level)
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
                        logger.{{{level}}}Interpolated($"Test {value.Named({|#0:name|})}, {value.Named({|#1:name|})}.");
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated($"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task LogException_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated(ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task WriteInterpolated_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, $"Test {value.Named({|#0:"{{name}}"|})}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4003").WithLocation(0).WithArguments(name.TrimOperator());
        await VerifyExplicitNamesCS.VerifyAnalyzerAsync(test, expected);
    }

    [Test]
    public async Task WriteInterpolatedException_WithInvalidName_ExplicitNameMustBeValidIdentifierDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
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
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated($"Test {value.Named({|#0:"{{name}}"|})}.");
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
                        logger.{{level}}Interpolated($"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task LogException_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.{{level}}Interpolated(ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
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
                        logger.{{level}}Interpolated(ex, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task WriteInterpolated_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, $"Test {value.Named({|#0:"{{name}}"|})}.");
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, $"Test {value.Named("{{name.Trim()}}")}.");
                    }
                }
            }
            """;

        DiagnosticResult expected = VerifyExplicitNamesCS.Diagnostic("ISLE4004").WithLocation(0).WithArguments(name);
        await VerifyExplicitNamesCS.VerifyCodeFixAsync(test, expected, fix);
    }

    [Test]
    public async Task WriteInterpolatedException_WithNameWithWhiteSpaces_ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId(
        [ValueSource(nameof(LogLevels))] LogEventLevel level,
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, ex, $"Test {value.Named({|#0:"{{name}}"|})}.");
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
                        logger.WriteInterpolated(LogEventLevel.{{level}}, ex, $"Test {value.Named("{{name.Trim()}}")}.");
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