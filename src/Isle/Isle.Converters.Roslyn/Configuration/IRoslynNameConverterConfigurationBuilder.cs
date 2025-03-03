using Isle.Converters.Roslyn;

namespace Isle.Configuration;

public interface IRoslynNameConverterConfigurationBuilder
{
    IRoslynNameConverterConfigurationBuilder CapitalizeFirstCharacter();

    IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(params string[] prefixes);
    
    IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(IEnumerable<string> prefixes, StringComparison stringComparison = StringComparison.Ordinal);

    IRoslynNameConverterConfigurationBuilder AddTransformation(Func<string, NameExpressionType, string> transformation);

    IRoslynNameConverterConfigurationBuilder WithFallback(Func<string, string> fallback);

    IRoslynNameConverterConfigurationBuilder WithMemoization(int maxCacheSize = int.MaxValue);
}