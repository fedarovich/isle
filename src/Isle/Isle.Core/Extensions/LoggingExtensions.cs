using Isle.Configuration;

namespace Isle.Extensions;

public static class LoggingExtensions
{
    public static NamedLogValue Named<T>(this T value, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name cannot be null or empty string.", nameof(name));

        return new NamedLogValue(
            value,
            name,
            IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>(),
            NamedLogValue.Flags.ExplicitName);
    }
}