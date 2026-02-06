using Isle.Converters.Roslyn;

#pragma warning disable IDE0130

namespace Isle.Configuration;

/// <summary>
/// The configuration builder for <see cref="RoslynNameConverter"/>.
/// </summary>
public interface IRoslynNameConverterConfigurationBuilder
{
    /// <summary>
    /// Enabled capitalization of the first letter.
    /// </summary>
    IRoslynNameConverterConfigurationBuilder CapitalizeFirstCharacter();

    /// <summary>
    /// Configures the converter to remove the <paramref name="prefixes"/> from the method names.
    /// </summary>
    IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(params string[] prefixes);

    /// <summary>
    /// Configures the converter to remove the <paramref name="prefixes"/> from the method names.
    /// </summary>
    IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(IEnumerable<string> prefixes, StringComparison stringComparison = StringComparison.Ordinal);

    /// <summary>
    /// Adds a custom name transformation.
    /// </summary>
    IRoslynNameConverterConfigurationBuilder AddTransformation(Func<string, NameExpressionType, string> transformation);

    /// <summary>
    /// Sets a fallback converter to be used when <see cref="RoslynNameConverter"/> cannot provide a valid result.
    /// </summary>
    /// <param name="fallback"></param>
    /// <returns></returns>
    IRoslynNameConverterConfigurationBuilder WithFallback(Func<string, string> fallback);

    /// <summary>
    /// Enables memoization and optionally sets the maximal cache size.
    /// </summary>
    /// <remarks>
    /// It's highly recommended to enable memoization for performance reasons.
    /// </remarks>
    IRoslynNameConverterConfigurationBuilder WithMemoization(int maxCacheSize = int.MaxValue);
}