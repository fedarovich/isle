using System.Diagnostics.CodeAnalysis;
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
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <param name="name">
    /// The name of the property in the message template. It may start with <c>@</c> (destructure) or <c>$</c> (stringify).
    /// It should be a constant, because the names are cached together with the message templates.
    /// </param>
    /// <returns>The <see cref="NamedLogValue"/> to be used in an interpolation expression.</returns>
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
    public static NamedLogValue Named<T>(this T value, [ConstantExpected] string name)
    {
        return Named(value, name, CoreConfiguration.PreserveDefaultValueRepresentationForExplicitNames);
    }

    /// <summary>
    /// Wraps the <paramref name="value"/> with an instance of <see cref="NamedLogValue"/> with the corresponding <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="T">The compile-time type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <param name="name">
    /// The name of the property in the message template. It may start with <c>@</c> (destructure) or <c>$</c> (stringify).
    /// It should be a constant, because the names are cached together with the message templates.
    /// </param>
    /// <param name="preserveDefaultValueRepresentation">
    /// If <see langword="true" /> the <paramref name="name"/> will be preserved as is;
    /// otherwise, depending on the <see cref="IsleConfiguration.ValueRepresentationPolicy"/>,
    /// the name can be prepended with <c>@</c> for destructuring or with <c>$</c> for stringification.
    /// </param>
    /// <returns>The <see cref="NamedLogValue"/> to be used in an interpolation expression.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or blank string.</exception>
    /// <seealso cref="Named{T}(T,string)"/>
    public static NamedLogValue Named<T>(this T value, [ConstantExpected] string name, bool preserveDefaultValueRepresentation)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name cannot be null or empty string.", nameof(name));

        string rawName = name;
        if (!preserveDefaultValueRepresentation && !name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            var representation = CoreConfiguration.GetRepresentationOfType<T>();
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
        var name = CoreConfiguration.ConvertValueName(expression);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            var representation = CoreConfiguration.GetRepresentationOfType<T>();
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