namespace Isle.Serilog.Configuration;

internal class SerilogConfiguration
{
    private static SerilogConfiguration? _current;

    public static SerilogConfiguration Current
    {
        get => _current ?? DefaultConfiguration.Value;
        set => _current = value;
    }

    public bool EnableMessageTemplateCaching { get; init; } = true;

    internal static class DefaultConfiguration
    {
        public static readonly SerilogConfiguration Value = new();

        static DefaultConfiguration()
        {
        }
    }
}