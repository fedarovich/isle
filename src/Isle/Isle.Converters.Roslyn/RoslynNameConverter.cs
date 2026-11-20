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
    /// <param name="fallback">
    /// A converter to be used if this converter cannot extract a name from the expression.
    /// It receives the original expression, and its result is used as is.
    /// </param>
    /// <param name="transformations">
    /// The transformations to be applied, in order, to the extracted name.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="fallback"/> or <paramref name="transformations"/> is <see langword="null"/>.
    /// </exception>
    /// <remarks>
    /// In most cases you don't need to create this class directly.
    /// Use <see cref="Isle.Configuration.RoslynNameConverterConfigurationExtensions.WithRoslynNameConverter"/> instead.
    /// </remarks>
    public RoslynNameConverter(Func<string, string> fallback, IEnumerable<Func<string, NameExpressionType, string>> transformations)
    {
        ArgumentNullException.ThrowIfNull(fallback);
        ArgumentNullException.ThrowIfNull(transformations);

        _fallback = fallback;
        _transformations = [.. transformations];
    }

    /// <summary>
    /// Extracts the member name from the C# <paramref name="expression"/> and applies the transformations to it.
    /// </summary>
    /// <param name="expression">The source code of the C# expression, as captured by the compiler.</param>
    /// <returns>
    /// The extracted and transformed name. If no name can be extracted from the <paramref name="expression"/>,
    /// or if parsing or a transformation fails, the result of the fallback converter applied to the original expression.
    /// </returns>
    /// <remarks>
    /// <para>
    /// The expression is unwrapped down to a simple name. Identifiers, member access, conditional member access,
    /// method invocations, casts, parentheses, increments and decrements, pointer indirection,
    /// the null-forgiving operator, and <c>checked</c>/<c>unchecked</c> expressions are supported.
    /// For example, <c>unchecked((short)value.IntProperty)</c> produces <c>IntProperty</c>.
    /// </para>
    /// <para>
    /// A leading <c>@</c> of a verbatim identifier is removed before the transformations are applied and added back afterwards.
    /// </para>
    /// </remarks>
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
