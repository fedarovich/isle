namespace Isle.Configuration;

/// <summary>
/// Describes how the values of certain types must be represented in logs.
/// </summary>
public interface IValueRepresentationPolicy
{
    /// <summary>
    /// Gets the representation of values of the type <typeparamref name="T"/>.
    /// </summary>
    ValueRepresentation GetRepresentationOfType<T>();
}