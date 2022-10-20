using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

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
            context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
        }

        private void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            if (context.Tree.Options is not CSharpParseOptions options)
                return;

            if (options.LanguageVersion < LanguageVersion.CSharp10)
            {
                var diagnostic = Diagnostic.Create(Rule, Location.None, options.LanguageVersion.ToDisplayString());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
