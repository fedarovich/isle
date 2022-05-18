using System.Runtime.CompilerServices;
using Isle.Configuration;

namespace Isle.Extensions;

/// <summary>
/// Provides extension methods related to logging functionality.
/// </summary>
public static class LoggingExtensions
{
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    /// <summary>
    /// Wraps the <paramref name="value"/> with an instance of <see cref="NamedLogValue"/> with the corresponding <paramref name="name"/>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or blank string.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NamedLogValue Named<T>(this T value, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name cannot be null or empty string.", nameof(name));

        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            var representation = IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>();
            switch (representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

        return new NamedLogValue(value, name);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetNameFromCallerArgumentExpression<T>(this string expression)
    {
        var name = IsleConfiguration.Current.ValueNameConverter(expression);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            var representation = IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>();
            switch (representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

        return name;
    }
}