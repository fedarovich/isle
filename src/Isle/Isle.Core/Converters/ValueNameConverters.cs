using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isle.Converters;

/// <summary>
/// Provides default implementation of value name converters.
/// </summary>
public static class ValueNameConverters
{
    /// <summary>
    /// Wraps another <paramref name="converter"/> to add memoization support.
    /// </summary>
    /// <param name="converter">The value name converter to wrap.</param>
    public static Func<string, string> WithMemoization(this Func<string, string> converter)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.
#endif
            ThrowIfNull(converter);

        var cache = new ConcurrentDictionary<string, string>();
        return expression => cache.GetOrAdd(expression, static (expr, conv) => conv(expr), converter);
    }

    /// <summary>
    /// Wraps another <paramref name="converter"/> to add memoization support.
    /// </summary>
    /// <param name="converter">The value name converter to wrap.</param>
    /// <param name="maxCacheSize">The maximal number of expressions to cache.</param>
    /// <remarks>
    /// <para>If <paramref name="maxCacheSize"/> is 0, the results produced by <paramref name="converter"/> won't be cached.</para>
    /// <para>If <paramref name="maxCacheSize"/> is <see cref="int.MaxValue"/>, all results produced by <paramref name="converter"/> will be cached.</para>
    /// <para>Otherwise, up to <paramref name="maxCacheSize"/> results produced by <paramref name="converter"/> will be cached.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
    public static Func<string, string> WithMemoization(this Func<string, string> converter, int maxCacheSize)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.
#endif
            ThrowIfNull(converter);
        return maxCacheSize switch
        {
            < 0 => throw new ArgumentException("MaxCacheSize must be greater than or equal to 0.", nameof(maxCacheSize)),
            0 => converter,
            int.MaxValue => WithMemoization(converter),
            _ => Create(converter, maxCacheSize)
        };

        static Func<string, string> Create(Func<string, string> converter, int maxCacheSize)
        {
            var cache = new ConcurrentDictionary<string, string>();
            return expression => cache.Count >= maxCacheSize
                ? (cache.TryGetValue(expression, out var value) ? value : converter(expression))
                : cache.GetOrAdd(expression, static (expr, conv) => conv(expr), converter);
        }
    }

    /// <summary>
    /// Returns a value name converter that removes all characters except letters, digits and underscore (<c>_</c>) from the string,
    /// and optionally capitalizes the first character if it is a lower case letter.
    /// </summary>
    /// <param name="capitalizeFirstCharacter">The value indicating whether the first character of the string must be capitalized.</param>
    /// <remarks>
    /// If <paramref name="capitalizeFirstCharacter"/> is <see langword="true"/> and the first character is <c>@</c>,
    /// the next character will be capitalized.
    /// </remarks>
    public static Func<string, string> SerilogCompatible(bool capitalizeFirstCharacter = true)
    {
        return new SerilogCompatibleValueNameConverter(capitalizeFirstCharacter).Convert;
    }

    /// <summary>
    /// Returns a value name converter that capitalizes the first character if it is a lower case letter.
    /// In case the first character is <c>@</c>, the next character will be capitalized.
    /// </summary>
    /// <returns></returns>
    public static Func<string, string> CapitalizeFirstCharacter() => CapitalizeFirstCharacterConverter.Convert;

#if !NET6_0_OR_GREATER
    /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    private static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw(paramName);
        }
    }

    [DoesNotReturn]
    private static void Throw(string? paramName) =>
        throw new ArgumentNullException(paramName);
#endif
}