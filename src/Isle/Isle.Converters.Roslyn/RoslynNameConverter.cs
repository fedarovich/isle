using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Isle.Converters.Roslyn;

/// <summary>
/// A name converter that extracts a property or method name from C# expression using Roslyn parser.
/// </summary>
public class RoslynNameConverter
{
    private readonly Func<string, string> _fallback;
    private readonly Func<string, NameExpressionType, string>[] _transformations;

    /// <summary>
    /// Initializes a new instance of <see cref="RoslynNameConverter"/>.
    /// </summary>
    /// <param name="fallback">A fallback to be used if this converter cannot produce a meaningful value.</param>
    /// <param name="transformations">Additional custom transformations to be applied to the converted name.</param>
    public RoslynNameConverter(Func<string, string> fallback, IEnumerable<Func<string, NameExpressionType, string>> transformations)
    {
        ArgumentNullException.ThrowIfNull(fallback);
        ArgumentNullException.ThrowIfNull(transformations);

        _fallback = fallback;
        _transformations = [.. transformations];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public string Convert(string expression)
    {
        try
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentException("The expression cannot be null or empty string.");

            var expressionSyntax = SyntaxFactory.ParseExpression(expression);

            var (nameSyntax, type) = Unwrap(expressionSyntax);

            if (nameSyntax is null)
                return _fallback(expression);

            var name = nameSyntax.ToString();
            

            if (_transformations is not [])
            {
                bool isPrefixed = name.StartsWith('@');
                if (isPrefixed)
                {
                    name = name.Substring(1);
                }

                foreach (var transformation in _transformations)
                {
                    name = transformation(name, type);
                }

                if (isPrefixed)
                {
                    name = '@' + name;
                }
            }

            return name;
        }
        catch
        {
            return _fallback(expression);
        }
    }

    private static (SimpleNameSyntax? syntax, NameExpressionType type) Unwrap(ExpressionSyntax? syntax)
    {
        var currentSyntax = syntax;
        var type = NameExpressionType.Identifier;

        while (currentSyntax is not null)
        {
            switch (currentSyntax)
            {
                case IdentifierNameSyntax identifierName:
                    return (identifierName, type);
                case ParenthesizedExpressionSyntax parenthesizedExpression:
                    currentSyntax = parenthesizedExpression.Expression;
                    break;
                case MemberAccessExpressionSyntax memberAccess:
                    currentSyntax = memberAccess.Name;
                    type = NameExpressionType.Property;
                    break;
                case ConditionalAccessExpressionSyntax { WhenNotNull: var whenNotNull }:
                    currentSyntax = whenNotNull;
                    type = NameExpressionType.Property;
                    break;
                case MemberBindingExpressionSyntax memberBinding:
                    currentSyntax = memberBinding.Name;
                    break;
                case InvocationExpressionSyntax invocation:
                    if (invocation.Expression is IdentifierNameSyntax { Identifier.Text: "nameof" })
                        return (null, NameExpressionType.Invalid);

                    currentSyntax = invocation.Expression switch
                    {
                        MemberAccessExpressionSyntax invocationMemberAccess => invocationMemberAccess.Name,
                        MemberBindingExpressionSyntax invocationMemberBinding => invocationMemberBinding.Name,
                        _ => invocation.Expression
                    };
                    type = NameExpressionType.Method;
                    break;
                case PrefixUnaryExpressionSyntax
                {
                    RawKind: (int)SyntaxKind.PointerIndirectionExpression
                        or (int)SyntaxKind.PreIncrementExpression or (int)SyntaxKind.PreDecrementExpression
                } pointerIndirection:
                    currentSyntax = pointerIndirection.Operand;
                    break;
                case PostfixUnaryExpressionSyntax
                {
                    RawKind: (int)SyntaxKind.SuppressNullableWarningExpression
                        or (int)SyntaxKind.PostIncrementExpression or (int)SyntaxKind.PostDecrementExpression
                } postfixUnaryExpression:
                    currentSyntax = postfixUnaryExpression.Operand;
                    break;
                case CastExpressionSyntax castExpression:
                    currentSyntax = castExpression.Expression;
                    break;
                case CheckedExpressionSyntax checkedExpression:
                    currentSyntax = checkedExpression.Expression;
                    break;
                default:
                    return (null, NameExpressionType.Invalid);
            }
        }

        return (null, NameExpressionType.Invalid);
    }
}
