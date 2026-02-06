using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Isle.Converters.Roslyn.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ExplicitNameShouldNotHaveWhiteSpacesCodeFixProvider))]
public sealed class ExplicitNameShouldNotHaveWhiteSpacesCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(
        CapturedNamesAnalyzer.ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId);

    public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        if (!diagnostic.Properties.TryGetValue("Name", out var name) || name is null)
            return;

        var argumentSyntax = root.FindNode(diagnosticSpan) as ArgumentSyntax;
        if (argumentSyntax is null)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.CapturedNameMustBeValidIdentifierCodeFixProviderTitle,
                createChangedDocument: ct => CreateChangedDocument(context.Document, argumentSyntax.Expression, name, ct),
                equivalenceKey: nameof(ExplicitNameShouldNotHaveWhiteSpacesCodeFixProvider)
            ),
            diagnostic);
    }

    private async Task<Document> CreateChangedDocument(Document document, ExpressionSyntax originalExpression,
        string name, CancellationToken cancellationToken)
    {
        var oldSyntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (oldSyntaxRoot is null)
            return document;

        var newExpression = LiteralExpression(
            SyntaxKind.StringLiteralExpression,
            Literal(name.Trim()));

        var newSyntaxRoot = oldSyntaxRoot.ReplaceNode(originalExpression, newExpression);
        return document.WithSyntaxRoot(newSyntaxRoot);
    }
}
