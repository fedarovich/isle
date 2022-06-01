using System.Collections.Concurrent;

namespace Isle.Converters;

/// <summary>
/// Provides default implementation of value name converters.
/// </summary>
public static class ValueNameConverters
{
    /// <summary>
    /// Creates a value name converter that wraps another <paramref name="converter"/>
    /// and adds caching support.
    /// </summary>
    /// <param name="converter">The value name converter to wrap.</param>
    public static Func<string, string> CreateCachingConverter(Func<string, string> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);

        var cache = new ConcurrentDictionary<string, string>();
        return expression => cache.GetOrAdd(expression, static (expr, conv) => conv(expr), converter);
    }

    /// <summary>
    /// Creates a value name converter that wraps another <paramref name="converter"/>
    /// and adds caching support.
    /// </summary>
    /// <param name="converter">The value name converter to wrap.</param>
    /// <param name="maxCacheSize">The maximal number of expressions to cache.</param>
    /// <remarks>
    /// <para>If <paramref name="maxCacheSize"/> is 0, the results produced by <paramref name="converter"/> won't be cached.</para>
    /// <para>If <paramref name="maxCacheSize"/> is <see cref="int.MaxValue"/>, all results produced by <paramref name="converter"/> will be cached.</para>
    /// <para>Otherwise, up to <paramref name="maxCacheSize"/> results produced by <paramref name="converter"/> will be cached.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
    public static Func<string, string> CreateCachingConverter(Func<string, string> converter, int maxCacheSize)
    {
        ArgumentNullException.ThrowIfNull(converter);
        return maxCacheSize switch
        {
            < 0 => throw new ArgumentException("MaxCacheSize must be greater than or equal to 0.", nameof(maxCacheSize)),
            0 => converter,
            int.MaxValue => CreateCachingConverter(converter),
            _ => CreateWithLimitedCache(converter, maxCacheSize)
        };

        static Func<string, string> CreateWithLimitedCache(Func<string, string> converter, int maxCacheSize)
        {
            var cache = new ConcurrentDictionary<string, string>();
            return expression => cache.Count > maxCacheSize
                ? converter(expression)
                : cache.GetOrAdd(expression, static (expr, conv) => conv(expr), converter);
        }
    }

    /// <summary>
    /// Creates a value name converter that removes all characters except letters, digits and underscore (<c>_</c>) from the string,
    /// and optionally capitalizes the first character if it is a lower case letter.
    /// </summary>
    /// <param name="capitalizeFirstCharacter">The value indicating whether the first character of the string must be capitalized.</param>
    /// <param name="maxCacheSize">The maximal number of value names to cache.</param>
    /// <remarks>
    /// <para>If <paramref name="maxCacheSize"/> is 0, the results produced by the converter won't be cached.</para>
    /// <para>If <paramref name="maxCacheSize"/> is <see cref="int.MaxValue"/>, all results produced by the converter will be cached.</para>
    /// <para>Otherwise, up to <paramref name="maxCacheSize"/> results produced by the converter will be cached.</para>
    /// </remarks>
    public static Func<string, string> CreateSerilogCompatibleConverter(
        bool capitalizeFirstCharacter = true,
        int maxCacheSize = 4096)
    {
        var converter = new SerilogCompatibleValueNameConverter(capitalizeFirstCharacter).Convert;
        return CreateCachingConverter(converter, maxCacheSize);
    }
}