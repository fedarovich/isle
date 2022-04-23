namespace Isle.Configuration;

/// <summary>
/// ISLE Configuration.
/// </summary>
public sealed class IsleConfiguration
{
    private static volatile IsleConfiguration? _current;

    /// <summary>
    /// Gets the current ISLE configuration or throws <see cref="InvalidOperationException"/> if ISLE has not been configured yet.
    /// </summary>
    /// <exception cref="InvalidOperationException">ISLE has not been configured yet.</exception>
    /// <seealso cref="Configure"/>
    public static IsleConfiguration Current =>
        _current ?? throw new InvalidOperationException(
            "Isle is not configured. Please, call IsleConfiguration.Configure before using Isle's logging methods.");

    private IsleConfiguration(IsleConfigurationBuilder builder)
    {
        ValueRepresentationPolicy = builder.ValueRepresentationPolicy ?? DefaultValueRepresentationPolicy.Instance;
        ValueNameConverter = builder.ValueNameConverter ?? (name => name);
    }

    /// <summary>
    /// Gets the value representation policy.
    /// </summary>
    /// <remarks>
    /// <see cref="DefaultValueRepresentationPolicy"/> is used by default.
    /// </remarks>
    public IValueRepresentationPolicy ValueRepresentationPolicy { get; }

    /// <summary>
    /// Gets the value name converter.
    /// </summary>
    /// <remarks>
    /// Identity transform is used by default.
    /// </remarks>
    public Func<string, string> ValueNameConverter { get; }

    /// <summary>
    /// Configures ISLE.
    /// </summary>
    /// <param name="buildConfiguration">The action used for configuration.</param>
    /// <exception cref="ArgumentNullException"><paramref name="buildConfiguration"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">ISLE has already been configured.</exception>
    public static void Configure(Action<IIsleConfigurationBuilder> buildConfiguration)
    {
        if (buildConfiguration == null)
            throw new ArgumentNullException(nameof(buildConfiguration));

        var builder = new IsleConfigurationBuilder();
        buildConfiguration(builder);
        var configuration = new IsleConfiguration(builder);
        var prevConfiguration = Interlocked.CompareExchange(ref _current, configuration, null);
        if (prevConfiguration != null)
        {
            throw new InvalidOperationException("Isle has already been configured.");
        }
    }

    // For unit testing.
    internal static void Reset()
    {
        _current = null;
    }

    private class IsleConfigurationBuilder : IIsleConfigurationBuilder
    {
        public IValueRepresentationPolicy? ValueRepresentationPolicy { get; set; }
        
        public Func<string, string>? ValueNameConverter { get; set; }
    }
}