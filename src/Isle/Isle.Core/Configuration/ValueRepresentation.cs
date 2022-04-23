namespace Isle.Configuration;

/// <summary>
/// Specifies how a value should be represented.
/// </summary>
public enum ValueRepresentation
{
    /// <summary>
    /// Stringify the value.
    /// </summary>
    Stringify = -1,

    /// <summary>
    /// Use default value representation.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Destructure the value.
    /// </summary>
    Destructure = 1
}