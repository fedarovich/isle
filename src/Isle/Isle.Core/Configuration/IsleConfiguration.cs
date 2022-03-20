namespace Isle.Configuration;

public sealed class IsleConfiguration
{
    private static volatile IsleConfiguration? _current;

    public static IsleConfiguration Current =>
        _current ?? throw new InvalidOperationException(
            "Isle is not configured. Please, call IsleConfiguration.Configure before using Isle's logging methods.");

    private IsleConfiguration(IsleConfigurationBuilder builder)
    {
        ValueRepresentationPolicy = builder.ValueRepresentationPolicy ?? DefaultValueRepresentationPolicy.Instance;
        ValueNameConverter = builder.ValueNameConverter ?? (name => name);
    }

    public IValueRepresentationPolicy ValueRepresentationPolicy { get; }

    public Func<string, string> ValueNameConverter { get; }

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