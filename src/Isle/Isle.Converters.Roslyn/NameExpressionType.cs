namespace Isle.Converters.Roslyn;

/// <summary>
/// Describes the type of argument name expression.
/// </summary>
public enum NameExpressionType
{
    /// <summary>
    /// The expression is not a valid name.
    /// </summary>
    Invalid,

    /// <summary>
    /// The expression is a C# identifier.
    /// </summary>
    Identifier,

    /// <summary>
    /// The expression is a C# property access expression.
    /// </summary>
    Property,

    /// <summary>
    /// The expression is a C# indexer access expression.
    /// </summary>
    Indexer,

    /// <summary>
    /// The expression is a C# method call expression.
    /// </summary>
    Method
}
