using System.Runtime.CompilerServices;

namespace Isle.Configuration;

/// <summary>
/// <see cref="IValueRepresentationPolicy"/> that uses <see cref="ValueRepresentation.Default"/> representation for all values.
/// </summary>
public sealed class DefaultValueRepresentationPolicy : IValueRepresentationPolicy
{
    /// <summary>
    /// Gets the singleton instance of the policy.
    /// </summary>
    public static DefaultValueRepresentationPolicy Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    static DefaultValueRepresentationPolicy()
    {
        Instance = new DefaultValueRepresentationPolicy();
    }

    private DefaultValueRepresentationPolicy()
    {
    }

    /// <inheritdoc cref="IValueRepresentationPolicy.GetRepresentationOfType{T}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueRepresentation GetRepresentationOfType<T>() => ValueRepresentation.Default;
}