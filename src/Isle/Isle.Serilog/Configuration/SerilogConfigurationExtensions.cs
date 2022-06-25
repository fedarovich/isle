using Isle.Serilog.Configuration;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Isle.Configuration;

/// <summary>
/// Provides extension methods for <see cref="IIsleConfigurationBuilder"/>.
/// </summary>
public static class SerilogConfigurationExtensions
{
    /// <summary>
    /// Configures the parameters for integration with <see cref="ILogger"/>.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="buildConfiguration"></param>
    /// <returns></returns>
    public static IIsleConfigurationBuilder ConfigureSerilog(this IIsleConfigurationBuilder @this, Action<ISerilogConfigurationBuilder>? buildConfiguration = null)
    {
        var builder = new ConfigurationBuilder();
        @this.RegisterExtensionConfigurationHook(builder);
        buildConfiguration?.Invoke(builder);
        return @this;
    }

    private class ConfigurationBuilder : ISerilogConfigurationBuilder, IIsleExtensionConfigurationHook
    {
        public bool EnableMessageTemplateCaching { get; set; } = true;

        public void ApplyExtensionConfiguration()
        {
            SerilogConfiguration.Current = new SerilogConfiguration
            {
                EnableMessageTemplateCaching = EnableMessageTemplateCaching
            };
        }

        public void ResetExtensionConfiguration()
        {
            SerilogConfiguration.Current = null!;
        }
    }
}