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

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CapturedNameMustBeValidIdentifierCodeFixProvider))]
public sealed class CapturedNameMustBeValidIdentifierCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(
        CapturedNamesAnalyzer.CapturedNameMustBeValidIdentifierDiagnosticId);

    public override FixAllProvider? GetFixAllProvider() => null; // Fix all is not supported

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        if (root.FindNode(diagnosticSpan) is not ExpressionSyntax interpolationExpression)
            return;

        context.RegisterCodeFix(
            CodeAction.Create(
                CodeFixResources.CapturedNameMustBeValidIdentifierCodeFixProviderTitle,
                createChangedDocument: ct => CreateChangedDocument(context.Document, interpolationExpression, ct),
                equivalenceKey: nameof(CapturedNameMustBeValidIdentifierCodeFixProvider)
            ),
            diagnostic);
    }

    private async Task<Document> CreateChangedDocument(Document document, ExpressionSyntax originalExpression, CancellationToken cancellationToken)
    {
        var oldSyntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (oldSyntaxRoot is null)
            return document;

        var newExpression = originalExpression is ParenthesizedExpressionSyntax
            ? originalExpression
            : ParenthesizedExpression(originalExpression);

        newExpression = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    newExpression,
                    IdentifierName("Named")))
            .AddArgumentListArguments(
                Argument(
                    LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("TODO_SPECIFY_NAME"))));

        var newSyntaxRoot = oldSyntaxRoot.ReplaceNode(originalExpression, newExpression);
        return document.WithSyntaxRoot(newSyntaxRoot);
    }
}
