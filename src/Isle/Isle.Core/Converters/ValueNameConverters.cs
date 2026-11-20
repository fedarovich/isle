using System.Collections.Concurrent;

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
    /// <returns>A converter that caches all results produced by <paramref name="converter"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
    public static Func<string, string> WithMemoization(this Func<string, string> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);

        var cache = new ConcurrentDictionary<string, string>();
        return expression => cache.GetOrAdd(expression, static (expr, conv) => conv(expr), converter);
    }

    /// <summary>
    /// Wraps another <paramref name="converter"/> to add memoization support.
    /// </summary>
    /// <param name="converter">The value name converter to wrap.</param>
    /// <param name="maxCacheSize">The maximal number of expressions to cache.</param>
    /// <returns>A converter that caches the results produced by <paramref name="converter"/>.</returns>
    /// <remarks>
    /// <para>If <paramref name="maxCacheSize"/> is 0, the results produced by <paramref name="converter"/> won't be cached.</para>
    /// <para>If <paramref name="maxCacheSize"/> is <see cref="int.MaxValue"/>, all results produced by <paramref name="converter"/> will be cached.</para>
    /// <para>Otherwise, up to <paramref name="maxCacheSize"/> results produced by <paramref name="converter"/> will be cached.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="maxCacheSize"/> is negative.</exception>
    public static Func<string, string> WithMemoization(this Func<string, string> converter, int maxCacheSize)
    {
        ArgumentNullException.ThrowIfNull(converter);

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
    /// <returns>The value name converter.</returns>
    /// <remarks>
    /// <para>
    /// A leading <c>@</c> character (one that comes before any letter, digit or underscore) is preserved,
    /// so that the destructuring operator is kept. Any other <c>@</c> character, such as the one in <c>obj.@class</c>, is removed.
    /// If <paramref name="capitalizeFirstCharacter"/> is <see langword="true"/> and the result starts with <c>@</c>,
    /// the next character will be capitalized.
    /// </para>
    /// <para>
    /// The converter throws <see cref="ArgumentException"/> if the name is empty or consists only of the removed characters.
    /// </para>
    /// </remarks>
    public static Func<string, string> SerilogCompatible(bool capitalizeFirstCharacter = true)
    {
        return new SerilogCompatibleValueNameConverter(capitalizeFirstCharacter).Convert;
    }

    /// <summary>
    /// Returns a value name converter that capitalizes the first character if it is a lower case letter.
    /// In case the first character is <c>@</c>, the next character will be capitalized.
    /// </summary>
    /// <returns>The value name converter.</returns>
    /// <remarks>
    /// The capitalization uses the invariant culture.
    /// </remarks>
    public static Func<string, string> CapitalizeFirstCharacter() => CapitalizeFirstCharacterConverter.Convert;
}