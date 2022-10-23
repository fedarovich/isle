using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Isle.Core.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LangVersionAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ISLE0001";

        private static readonly LocalizableString Title = new LocalizableResourceString(
            nameof(Resources.LangVersionAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(
            nameof(Resources.LangVersionAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(
            nameof(Resources.LangVersionAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Compiler";

        private static readonly DiagnosticDescriptor Rule = new (
            DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(ctx => AnalyzeCompilation(ctx));
        }

        private void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            if (context.Compilation is not CSharpCompilation compilation)
                return;

            if (compilation.LanguageVersion < LanguageVersion.CSharp10)
            {
                var diagnostic = Diagnostic.Create(Rule, Location.None, compilation.LanguageVersion.ToDisplayString());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
