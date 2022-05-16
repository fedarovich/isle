namespace Isle.Extensions.Logging.Configuration;

internal class ExtensionsLoggingConfiguration
{
    private static ExtensionsLoggingConfiguration? _current;

    public static ExtensionsLoggingConfiguration Current
    {
        get => _current ?? DefaultConfiguration.Value;
        set => _current = value;
    }

    public bool EnableMessageTemplateCaching { get; init; } = true;

    private static class DefaultConfiguration
    {
        public static readonly ExtensionsLoggingConfiguration Value = new();

        static DefaultConfiguration()
        {
        }
    }
}