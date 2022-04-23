namespace Isle.Configuration;

/// <summary>
/// Provides the properties and methods to configure ISLE.
/// </summary>
public interface IIsleConfigurationBuilder
{
    /// <summary>
    /// Gets or sets the value representation policy.
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/>, <see cref="DefaultValueRepresentationPolicy"/> will be used.
    /// </remarks>
    IValueRepresentationPolicy? ValueRepresentationPolicy { get; set; }

    /// <summary>
    /// Gets or sets the value name converter.
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/>, a function returning original value will be used.
    /// </remarks>
    Func<string, string>? ValueNameConverter { get; set; }
}