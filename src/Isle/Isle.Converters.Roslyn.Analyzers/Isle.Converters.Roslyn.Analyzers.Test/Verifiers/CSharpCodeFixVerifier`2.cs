using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using System.Threading;
using System.Threading.Tasks;

namespace Isle.Converters.Roslyn.Analyzers.Test
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        private const string CapturedNamesAnalyzerEditorConfig =
            """
            is_global = true
            
            build_property.IsleRoslynNameConverterRemoveMethodPrefixes = Is Get
            """;

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic()"/>
        public static DiagnosticResult Diagnostic()
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic();

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(string)"/>
        public static DiagnosticResult Diagnostic(string diagnosticId)
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic(diagnosticId);

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)"/>
        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic(descriptor);

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
        public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
        {
            var test = new Test
            {
                TestCode = source,
                ReferenceAssemblies = ReferenceAssemblies.Net.Net100,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/CapturedNamesAnalyzer.editorconfig", CapturedNamesAnalyzerEditorConfig)
                    }
                }
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync(CancellationToken.None);
        }

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, string)"/>
        public static async Task VerifyCodeFixAsync(string source, string fixedSource)
            => await VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult, string)"/>
        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
            => await VerifyCodeFixAsync(source, [expected], fixedSource);

        /// <inheritdoc cref="CodeFixVerifier{TAnalyzer, TCodeFix, TTest, TVerifier}.VerifyCodeFixAsync(string, DiagnosticResult[], string)"/>
        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource)
            => await VerifyCodeFixAsync(source, expected, fixedSource, codeActionVerifier: null);

        /// <summary>
        /// Verifies the code fix like <see cref="VerifyCodeFixAsync(string, DiagnosticResult, string)"/>
        /// and checks that the applied code action has the expected title.
        /// </summary>
        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource, string expectedCodeActionTitle)
            => await VerifyCodeFixAsync(source, [expected], fixedSource,
                (codeAction, verifier) => verifier.Equal(expectedCodeActionTitle, codeAction.Title, "Unexpected code action title."));

        private static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource,
            Action<CodeAction, IVerifier> codeActionVerifier)
        {
            var test = new Test
            {
                TestCode = source,
                FixedCode = fixedSource,
                ReferenceAssemblies = ReferenceAssemblies.Net.Net100,
                TestState =
                {
                    AnalyzerConfigFiles =
                    {
                        ("/CapturedNamesAnalyzer.editorconfig", CapturedNamesAnalyzerEditorConfig)
                    }
                },
                CodeActionVerifier = codeActionVerifier
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync(CancellationToken.None);
        }
    }
}
