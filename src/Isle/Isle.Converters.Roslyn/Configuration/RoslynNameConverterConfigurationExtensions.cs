using Isle.Converters;
using Isle.Converters.Roslyn;

#pragma warning disable IDE0130

namespace Isle.Configuration;

/// <summary>
/// Provides extension methods to simplify adding <see cref="RoslynNameConverter"/> to Isle configuration.
/// </summary>
public static class RoslynNameConverterConfigurationExtensions
{
    /// <summary>
    /// Assigns the <see cref="RoslynNameConverter"/>> to the <see cref="IIsleConfigurationBuilder.ValueNameConverter"/> property.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="configure"/> is <see langword="null"/>.</exception>
    public static IIsleConfigurationBuilder WithRoslynNameConverter(this IIsleConfigurationBuilder builder, Action<IRoslynNameConverterConfigurationBuilder> configure)
    {
        if (configure is null)
            throw new ArgumentNullException(nameof(configure));

        var configBuilder = new RoslynNameConverterConfigurationBuilder();
        configure(configBuilder);
        
        var converter = configBuilder.Build().Convert;
        builder.ValueNameConverter = converter.WithMemoization(configBuilder.MemoizationCacheSize);
        return builder;
    }
}
