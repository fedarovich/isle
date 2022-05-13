using Isle.Configuration;

namespace Isle.Extensions;

/// <summary>
/// Provides extension methods related to logging functionality.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Wraps the <paramref name="value"/> with an instance of <see cref="NamedLogValue"/> with the corresponding <paramref name="name"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or blank string.</exception>
    public static NamedLogValue Named<T>(this T value, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name cannot be null or empty string.", nameof(name));

        return new NamedLogValue(
            value,
            name,
            typeof(T),
            IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(),
            NamedLogValue.Flags.ExplicitName);
    }
}