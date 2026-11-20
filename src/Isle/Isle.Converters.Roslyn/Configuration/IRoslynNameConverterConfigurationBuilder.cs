using Isle.Converters.Roslyn;

#pragma warning disable IDE0130

namespace Isle.Configuration;

/// <summary>
/// The configuration builder for <see cref="RoslynNameConverter"/>.
/// </summary>
/// <remarks>
/// The transformations added by <see cref="CapitalizeFirstCharacter"/>, <see cref="RemoveMethodPrefixes(string[])"/>
/// and <see cref="AddTransformation"/> are applied in the order they were added.
/// </remarks>
public interface IRoslynNameConverterConfigurationBuilder
{
    /// <summary>
    /// Enables capitalization of the first letter of the name.
    /// </summary>
    /// <returns>This builder, for chaining.</returns>
    IRoslynNameConverterConfigurationBuilder CapitalizeFirstCharacter();

    /// <summary>
    /// Configures the converter to remove the <paramref name="prefixes"/> from the method names,
    /// using ordinal case-sensitive comparison.
    /// </summary>
    /// <param name="prefixes">The prefixes to remove, for example <c>Get</c>. Only the first matching prefix is removed.</param>
    /// <returns>This builder, for chaining.</returns>
    /// <remarks>
    /// The prefixes are removed only from the names of invoked methods, never from property or variable names.
    /// </remarks>
    IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(params string[] prefixes);

    /// <summary>
    /// Configures the converter to remove the <paramref name="prefixes"/> from the method names.
    /// </summary>
    /// <param name="prefixes">The prefixes to remove, for example <c>Get</c>. Only the first matching prefix is removed.</param>
    /// <param name="stringComparison">The comparison used to match the prefixes.</param>
    /// <returns>This builder, for chaining.</returns>
    /// <remarks>
    /// The prefixes are removed only from the names of invoked methods, never from property or variable names.
    /// </remarks>
    IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(IEnumerable<string> prefixes, StringComparison stringComparison = StringComparison.Ordinal);

    /// <summary>
    /// Adds a custom name transformation.
    /// </summary>
    /// <param name="transformation">
    /// The transformation. It receives the current name and the kind of expression the name was extracted from,
    /// and returns the transformed name.
    /// </param>
    /// <returns>This builder, for chaining.</returns>
    IRoslynNameConverterConfigurationBuilder AddTransformation(Func<string, NameExpressionType, string> transformation);

    /// <summary>
    /// Sets a fallback converter to be used when <see cref="RoslynNameConverter"/> cannot provide a valid result.
    /// </summary>
    /// <param name="fallback">
    /// The fallback converter. It receives the original expression, and its result is used as is,
    /// without applying the transformations. By default, the expression is returned unchanged.
    /// </param>
    /// <returns>This builder, for chaining.</returns>
    IRoslynNameConverterConfigurationBuilder WithFallback(Func<string, string> fallback);

    /// <summary>
    /// Enables memoization and optionally sets the maximal cache size.
    /// </summary>
    /// <param name="maxCacheSize">
    /// The maximal number of results to cache. <see cref="int.MaxValue"/> means no limit; 0 disables memoization.
    /// </param>
    /// <returns>This builder, for chaining.</returns>
    /// <remarks>
    /// Memoization is disabled unless this method is called.
    /// It's highly recommended to enable memoization for performance reasons.
    /// </remarks>
    IRoslynNameConverterConfigurationBuilder WithMemoization(int maxCacheSize = int.MaxValue);
}
