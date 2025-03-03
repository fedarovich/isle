using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Isle.Converters.Roslyn.Analyzers.Test
{
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new()
    {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
        {
            public Test()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    var compilationOptions = solution.GetProject(projectId).CompilationOptions;
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
                    solution = solution
                        .WithProjectCompilationOptions(projectId, compilationOptions)
                        .AddMetadataReferences(
                            projectId,
                            [
                                CreateMetadataReference<Microsoft.Extensions.Logging.ILogger>(),
                                CreateMetadataReference<Isle.NamedLogValue>(),
                                CreateMetadataReference<Isle.Extensions.Logging.InformationLogInterpolatedStringHandler>()
                            ]);

                    return solution;
                });
            }

            private static MetadataReference CreateMetadataReference<T>() where T : allows ref struct
            {
                return MetadataReference.CreateFromFile(typeof(T).Assembly.GetAssemblyLocation());
            }
        }
    }
}
