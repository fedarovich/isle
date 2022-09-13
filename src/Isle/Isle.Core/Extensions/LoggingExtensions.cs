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
    /// <param name="value">The value to wrap.</param>
    /// <param name="name">The name.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or blank string.</exception>
    /// <remarks>
    /// <para>
    /// This method is equivalent to:
    /// <code>
    /// Named(value, name, IsleConfiguration.Current.PreserveDefaultValueRepresentationForExplicitNames)
    /// </code>
    /// </para>
    /// <para>
    /// If <see cref="IsleConfiguration.PreserveDefaultValueRepresentationForExplicitNames"/> is <see langword="true" />
    /// the <paramref name="name"/> will be preserved as is;
    /// otherwise, depending on the <see cref="IsleConfiguration.ValueRepresentationPolicy"/>,
    /// the name can be prepended with <c>@</c> for destructuring or with <c>$</c> for stringification.
    /// </para>
    /// <para>By default, <see cref="IsleConfiguration.PreserveDefaultValueRepresentationForExplicitNames"/> is <see langword="false" />.</para>
    /// </remarks>
    /// <seealso cref="Named{T}(T,string,bool)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NamedLogValue Named<T>(this T value, string name)
    {
        var configuration = IsleConfiguration.Current;
        return Named(value, name, configuration.PreserveDefaultValueRepresentationForExplicitNames, configuration);
    }

    /// <summary>
    /// Wraps the <paramref name="value"/> with an instance of <see cref="NamedLogValue"/> with the corresponding <paramref name="name"/>.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="name">The name.</param>
    /// <param name="preserveDefaultValueRepresentation">
    /// If <see langword="true" /> the <paramref name="name"/> will be preserved as is;
    /// otherwise, depending on the <see cref="IsleConfiguration.ValueRepresentationPolicy"/>,
    /// the name can be prepended with <c>@</c> for destructuring or with <c>$</c> for stringification.
    /// </param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or blank string.</exception>
    /// <seealso cref="Named{T}(T,string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NamedLogValue Named<T>(this T value, string name, bool preserveDefaultValueRepresentation)
    {
        return Named(value, name, preserveDefaultValueRepresentation, IsleConfiguration.Current);
    }

    private static NamedLogValue Named<T>(T value, string name, bool preserveDefaultValueRepresentation, IsleConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name cannot be null or empty string.", nameof(name));

        string rawName = name;
        if (!preserveDefaultValueRepresentation && !name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            var representation = configuration.ValueRepresentationPolicy.GetRepresentationOfType<T>();
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

        return new NamedLogValue(value, name, rawName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetNameFromCallerArgumentExpression<T>(this string expression)
    {
        var configuration = IsleConfiguration.Current;
        var name = configuration.ValueNameConverter(expression);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            var representation = configuration.ValueRepresentationPolicy.GetRepresentationOfType<T>();
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