using Isle.Converters;
using Isle.Converters.Roslyn;

namespace Isle.Configuration;

internal class RoslynNameConverterConfigurationBuilder : IRoslynNameConverterConfigurationBuilder
{
    private Func<string, string>? _fallback;
    private readonly List<Func<string, NameExpressionType, string>> _transformations = [];

    internal int MemoizationCacheSize { get; set; }

    public IRoslynNameConverterConfigurationBuilder CapitalizeFirstCharacter()
    {
        return AddTransformation((name, _) => CapitalizeFirstCharacterConverter.Convert(name));
    }

    public IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(params string[] prefixes)
    {
        return RemoveMethodPrefixes(prefixes, StringComparison.Ordinal);
    }

    public IRoslynNameConverterConfigurationBuilder RemoveMethodPrefixes(IEnumerable<string> prefixes, StringComparison stringComparison = StringComparison.Ordinal)
    {
        if (prefixes is null)
            throw new ArgumentNullException(nameof(prefixes));

        var transformation = new RemoveMethodPrefixesTransformation(prefixes, stringComparison);
        return AddTransformation(transformation.Transform);
    }

    public IRoslynNameConverterConfigurationBuilder AddTransformation(Func<string, NameExpressionType, string> transformation)
    {
        _transformations.Add(transformation ?? throw new ArgumentNullException(nameof(transformation)));
        return this;
    }

    public IRoslynNameConverterConfigurationBuilder WithFallback(Func<string, string> fallback)
    {
        _fallback = fallback ?? throw new ArgumentNullException(nameof(fallback));
        return this;
    }

    public IRoslynNameConverterConfigurationBuilder WithMemoization(int maxCacheSize = int.MaxValue)
    {
        MemoizationCacheSize = maxCacheSize;
        return this;
    }

    public RoslynNameConverter Build()
    {
        return new RoslynNameConverter(
            _fallback ?? (name => name),
            _transformations);
    }
}
