using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Isle.Converters.Roslyn.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CapturedNamesAnalyzer : DiagnosticAnalyzer
{
    private static readonly string[] RemoveMethodPrefixesSeparators = [";", " "];
    private const string RemoveMethodPrefixesOptionName = "IsleRoslynNameConverterRemoveMethodPrefixes";
    private const string RemoveMethodPrefixesStringComparisonOptionName = "IsleRoslynNameConverterRemoveMethodPrefixesStringComparison";

    public const string CapturedNameMustBeValidIdentifierDiagnosticId = "ISLE4000";
    private static readonly LocalizableString CapturedNameMustBeValidIdentifierTitle = GetResourceString(nameof(Resources.CapturedNameMustBeValidIdentifierTitle));
    private static readonly LocalizableString CapturedNameMustBeValidIdentifierMessageFormat = GetResourceString(nameof(Resources.CapturedNameMustBeValidIdentifierMessageFormat));
    private static readonly LocalizableString CapturedNameMustBeValidIdentifierDescription = GetResourceString(nameof(Resources.CapturedNameMustBeValidIdentifierDescription));
    private static readonly DiagnosticDescriptor CapturedNameMustBeValidIdentifierRule = new (
        CapturedNameMustBeValidIdentifierDiagnosticId, CapturedNameMustBeValidIdentifierTitle, CapturedNameMustBeValidIdentifierMessageFormat, "Usage", DiagnosticSeverity.Warning, 
        isEnabledByDefault: true, description: CapturedNameMustBeValidIdentifierDescription);

    public const string CapturedNameMustBeUniqueDiagnosticId = "ISLE4001";
    private static readonly LocalizableString CapturedNameMustBeUniqueTitle = GetResourceString(nameof(Resources.CapturedNameMustBeUniqueTitle));
    private static readonly LocalizableString CapturedNameMustBeUniqueMessageFormat = GetResourceString(nameof(Resources.CapturedNameMustBeUniqueMessageFormat));
    private static readonly LocalizableString CapturedNameMustBeUniqueDescription = GetResourceString(nameof(Resources.CapturedNameMustBeUniqueDescription));
    private static readonly DiagnosticDescriptor CapturedNameMustBeUniqueRule = new(
        CapturedNameMustBeUniqueDiagnosticId, CapturedNameMustBeUniqueTitle, CapturedNameMustBeUniqueMessageFormat, "Usage", DiagnosticSeverity.Warning,
        isEnabledByDefault: true, description: CapturedNameMustBeUniqueDescription);

    public const string ExplicitNameShouldBeConstantDiagnosticId = "ISLE4002";
    private static readonly LocalizableString ExplicitNameShouldBeConstantTitle = GetResourceString(nameof(Resources.ExplicitNameShouldBeConstantTitle));
    private static readonly LocalizableString ExplicitNameShouldBeConstantMessageFormat = GetResourceString(nameof(Resources.ExplicitNameShouldBeConstantMessageFormat));
    private static readonly LocalizableString ExplicitNameShouldBeConstantDescription = GetResourceString(nameof(Resources.ExplicitNameShouldBeConstantDescription));
    private static readonly DiagnosticDescriptor ExplicitNameShouldBeConstantRule = new(
        ExplicitNameShouldBeConstantDiagnosticId, ExplicitNameShouldBeConstantTitle, ExplicitNameShouldBeConstantMessageFormat, "Usage", DiagnosticSeverity.Warning,
        isEnabledByDefault: true, description: ExplicitNameShouldBeConstantDescription);

    public const string ExplicitNameMustBeValidIdentifierDiagnosticId = "ISLE4003";
    private static readonly LocalizableString ExplicitNameMustBeValidIdentifierTitle = GetResourceString(nameof(Resources.ExplicitNameMustBeValidIdentifierTitle));
    private static readonly LocalizableString ExplicitNameMustBeValidIdentifierMessageFormat = GetResourceString(nameof(Resources.ExplicitNameMustBeValidIdentifierMessageFormat));
    private static readonly LocalizableString ExplicitNameMustBeValidIdentifierDescription = GetResourceString(nameof(Resources.ExplicitNameMustBeValidIdentifierDescription));
    private static readonly DiagnosticDescriptor ExplicitNameMustBeValidIdentifierRule = new(
        ExplicitNameMustBeValidIdentifierDiagnosticId, ExplicitNameMustBeValidIdentifierTitle, ExplicitNameMustBeValidIdentifierMessageFormat, "Usage", DiagnosticSeverity.Warning,
        isEnabledByDefault: true, description: ExplicitNameMustBeValidIdentifierDescription);

    public const string ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId = "ISLE4004";
    private static readonly LocalizableString ExplicitNameShouldNotHaveWhiteSpacesTitle = GetResourceString(nameof(Resources.ExplicitNameShouldNotHaveWhiteSpacesTitle));
    private static readonly LocalizableString ExplicitNameShouldNotHaveWhiteSpacesMessageFormat = GetResourceString(nameof(Resources.ExplicitNameShouldNotHaveWhiteSpacesMessageFormat));
    private static readonly LocalizableString ExplicitNameShouldNotHaveWhiteSpacesDescription = GetResourceString(nameof(Resources.ExplicitNameShouldNotHaveWhiteSpacesDescription));
    private static readonly DiagnosticDescriptor ExplicitNameShouldNotHaveWhiteSpacesRule = new(
        ExplicitNameShouldNotHaveWhiteSpacesDiagnosticId, ExplicitNameShouldNotHaveWhiteSpacesTitle, ExplicitNameShouldNotHaveWhiteSpacesMessageFormat, "Usage", DiagnosticSeverity.Warning,
        isEnabledByDefault: true, description: ExplicitNameShouldNotHaveWhiteSpacesDescription);

    private static LocalizableResourceString GetResourceString(string key)
    {
        return new LocalizableResourceString(key, Resources.ResourceManager, typeof(Resources));
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        CapturedNameMustBeValidIdentifierRule,
        CapturedNameMustBeUniqueRule,
        ExplicitNameShouldBeConstantRule,
        ExplicitNameMustBeValidIdentifierRule,
        ExplicitNameShouldNotHaveWhiteSpacesRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(OnCompilationStart);
    }

    private void OnCompilationStart(CompilationStartAnalysisContext compilationContext)
    {
        HashSet<ISymbol>? methods = null;

        var melExtensionsSymbol = compilationContext.Compilation.GetTypeByMetadataName("Isle.Extensions.Logging.LoggerExtensions");
        if (melExtensionsSymbol is not null)
        {
            methods = new HashSet<ISymbol>(SymbolEqualityComparer.Default);
            foreach (var member in melExtensionsSymbol.GetMembers())
            {
                methods.Add(member);
            }
        }

        var serilogExtensionsSymbol = compilationContext.Compilation.GetTypeByMetadataName("Isle.Serilog.LoggerExtensions");
        if (serilogExtensionsSymbol is not null)
        {
            methods ??= new HashSet<ISymbol>(SymbolEqualityComparer.Default);
            foreach (var member in serilogExtensionsSymbol.GetMembers())
            {
                methods.Add(member);
            }
        }

        var loggingExtensionsSymbol = compilationContext.Compilation.GetTypeByMetadataName("Isle.Extensions.LoggingExtensions");
        var namedMethods = loggingExtensionsSymbol?.GetMembers("Named") ?? ImmutableArray<ISymbol>.Empty;

        var literalValueSymbol = compilationContext.Compilation.GetTypeByMetadataName("Isle.LiteralValue");
            
        if (methods?.Count > 0)
        {
            compilationContext.RegisterOperationAction(
                context =>
                {
                    var invocation = (IInvocationOperation)context.Operation;
                    var methodSymbol = invocation.TargetMethod;

                    if (!methods.Contains(methodSymbol))
                        return;

                    var handlerArgumentSyntax = invocation.Arguments
                        .FirstOrDefault(a => a.Parameter?.Name == "handler")?.Syntax;
                    if (handlerArgumentSyntax is null)
                        return;

                    var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var interpolations = GetTopLevelStringInterpolationNodes(handlerArgumentSyntax)
                        .SelectMany(s => s.ChildNodes().OfType<InterpolationSyntax>());
                    foreach (var interpolation in interpolations)
                    {
                        if (!TryGetName(interpolation.Expression, out var name, out var expressionType))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                CapturedNameMustBeValidIdentifierRule,
                                interpolation.Expression.GetLocation(),
                                interpolation.Expression.ToString()));
                        }

                        if (name is null)
                            continue;

                        name = name.TrimStart('@', '$');

                        var options = context.Options.AnalyzerConfigOptionsProvider.GetOptions(context.Operation.Syntax.SyntaxTree);
                        if (expressionType == NameExpressionType.Method &&
                            options.TryGetValue(RemoveMethodPrefixesOptionName, out var removeMethodPrefixesValue) &&
                            !string.IsNullOrWhiteSpace(removeMethodPrefixesValue))
                        {
                            var prefixes = removeMethodPrefixesValue.Split(RemoveMethodPrefixesSeparators, StringSplitOptions.RemoveEmptyEntries);
                            if (prefixes.Length > 0)
                            {
                                const StringComparison defaultStringComparison = StringComparison.Ordinal;
                                var stringComparison = options.TryGetValue(RemoveMethodPrefixesStringComparisonOptionName, out var comparisonString)
                                    ? Enum.TryParse(comparisonString, true, out StringComparison comparison)
                                        ? comparison
                                        : defaultStringComparison
                                    : defaultStringComparison;
                                name = RemoveMethodPrefixes(name, prefixes, stringComparison);
                            }
                        }

                        if (!usedNames.Add(name))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(
                                CapturedNameMustBeUniqueRule,
                                interpolation.Expression.GetLocation(),
                                name));
                        }
                    }

                    bool TryGetName(ExpressionSyntax? syntax, out string? name, out NameExpressionType expressionType)
                    {
                        var semanticModel = invocation.SemanticModel;

                        name = null;
                        expressionType = NameExpressionType.Identifier;
                        var currentSyntax = syntax;

                        while (currentSyntax is not null)
                        {
                            switch (currentSyntax)
                            {
                                case IdentifierNameSyntax identifierName:
                                    name = identifierName.ToString();
                                    return true;
                                case ParenthesizedExpressionSyntax parenthesizedExpression:
                                    currentSyntax = parenthesizedExpression.Expression;
                                    break;
                                case MemberAccessExpressionSyntax memberAccess:
                                    currentSyntax = memberAccess.Name;
                                    expressionType = NameExpressionType.Property;
                                    break;
                                case ConditionalAccessExpressionSyntax { WhenNotNull: var whenNotNull }:
                                    currentSyntax = whenNotNull;
                                    expressionType = NameExpressionType.Property;
                                    break;
                                case MemberBindingExpressionSyntax memberBinding:
                                    currentSyntax = memberBinding.Name;
                                    break;
                                case InvocationExpressionSyntax invocationExpression:
                                    if (invocationExpression.Expression is IdentifierNameSyntax { Identifier.Text: "nameof" })
                                    {
                                        expressionType = NameExpressionType.Invalid;
                                        return false;
                                    }

                                    if (invocationExpression.Expression is MemberAccessExpressionSyntax { Name: IdentifierNameSyntax { Identifier.Text: "Named" } })
                                    {
                                        var operation = semanticModel?.GetOperation(invocationExpression, context.CancellationToken);
                                        if (operation is IInvocationOperation invocationOperation)
                                        {
                                            if (namedMethods.Contains(invocationOperation.TargetMethod.OriginalDefinition) && invocationOperation.Arguments.Length > 1)
                                            {
                                                var nameArgumentValue = invocationOperation.Arguments[1].Value;
                                                if (nameArgumentValue.ConstantValue is { HasValue: true, Value: string nameLiteral })
                                                {
                                                    name = nameLiteral;

                                                    if (string.IsNullOrWhiteSpace(nameLiteral))
                                                    {
                                                        context.ReportDiagnostic(Diagnostic.Create(
                                                            ExplicitNameMustBeValidIdentifierRule,
                                                            nameArgumentValue.Syntax.GetLocation(),
                                                            nameLiteral));
                                                    }
                                                    else
                                                    {
                                                        var trimmedNameLiteral = nameLiteral.Trim();
                                                        if (trimmedNameLiteral.Length != name.Length)
                                                        {
                                                            context.ReportDiagnostic(Diagnostic.Create(
                                                                ExplicitNameShouldNotHaveWhiteSpacesRule,
                                                                nameArgumentValue.Syntax.GetLocation(),
                                                                ImmutableDictionary.Create<string, string?>().Add("Name", nameLiteral),
                                                                nameLiteral));
                                                        }

                                                        if (trimmedNameLiteral[0] == '@' ||
                                                            trimmedNameLiteral[0] == '$')
                                                        {
                                                            trimmedNameLiteral = trimmedNameLiteral.Substring(1);
                                                        }

                                                        if (!SyntaxFacts.IsValidIdentifier(trimmedNameLiteral))
                                                        {
                                                            context.ReportDiagnostic(Diagnostic.Create(
                                                                ExplicitNameMustBeValidIdentifierRule,
                                                                nameArgumentValue.Syntax.GetLocation(),
                                                                trimmedNameLiteral));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    context.ReportDiagnostic(Diagnostic.Create(
                                                        ExplicitNameShouldBeConstantRule,
                                                        nameArgumentValue.Syntax.GetLocation()));
                                                }

                                                expressionType = NameExpressionType.Explicit;
                                                return true;
                                            }
                                        }
                                    }

                                    currentSyntax = invocationExpression.Expression switch
                                    {
                                        MemberAccessExpressionSyntax invocationMemberAccess => invocationMemberAccess.Name,
                                        MemberBindingExpressionSyntax invocationMemberBinding => invocationMemberBinding.Name,
                                        _ => invocationExpression.Expression
                                    };
                                    expressionType = NameExpressionType.Method;
                                    break;
                                case PrefixUnaryExpressionSyntax
                                {
                                    RawKind: (int)SyntaxKind.PointerIndirectionExpression
                                    or (int)SyntaxKind.PreIncrementExpression
                                    or (int)SyntaxKind.PreDecrementExpression
                                } pointerIndirection:
                                    currentSyntax = pointerIndirection.Operand;
                                    break;
                                case PostfixUnaryExpressionSyntax
                                {
                                    RawKind: (int)SyntaxKind.SuppressNullableWarningExpression
                                    or (int)SyntaxKind.PostIncrementExpression
                                    or (int)SyntaxKind.PostDecrementExpression
                                } postfixUnaryExpression:
                                    currentSyntax = postfixUnaryExpression.Operand;
                                    break;
                                case CastExpressionSyntax castExpression:
                                    currentSyntax = castExpression.Expression;
                                    break;
                                case CheckedExpressionSyntax checkedExpression:
                                    currentSyntax = checkedExpression.Expression;
                                    break;
                                case ObjectCreationExpressionSyntax objectCreation:
                                    if (semanticModel is null)
                                        return false;

                                    var createdTypeInfo = semanticModel.GetTypeInfo(objectCreation);
                                    return SymbolEqualityComparer.Default.Equals(createdTypeInfo.Type, literalValueSymbol);
                                default:
                                    expressionType = NameExpressionType.Invalid;
                                    return false;
                            }
                        }

                        return false;
                    }

                    string RemoveMethodPrefixes(string name, string[] prefixes, StringComparison stringComparison)
                    {
                        foreach (var prefix in prefixes)
                        {
                            if (name.StartsWith(prefix, stringComparison))
                            {
                                return name.Substring(prefix.Length);
                            }
                        }

                        return name;
                    }
                },
                OperationKind.Invocation);
        }
    }



    private IReadOnlyList<InterpolatedStringExpressionSyntax> GetTopLevelStringInterpolationNodes(SyntaxNode syntaxNode)
    {
        var result = new List<InterpolatedStringExpressionSyntax>();

        var stack = new Stack<SyntaxNode>();
        stack.Push(syntaxNode);
        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (node is InterpolatedStringExpressionSyntax interpolatedString)
            {
                result.Add(interpolatedString);
                continue;
            }

            foreach (var childNode in node.ChildNodes())
            {
                stack.Push(childNode);
            }
        }

        return result;
    }

    private enum NameExpressionType
    {
        Invalid,
        Identifier,
        Property,
        Method,
        Explicit
    }
}