using Isle.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Isle.Configuration;

/// <summary>
/// Provides extension methods for <see cref="IIsleConfigurationBuilder"/>.
/// </summary>
public static class ExtensionsLoggingConfigurationExtensions
{
    /// <summary>
    /// Configures the parameters for integration with <see cref="ILogger"/>.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="buildConfiguration"></param>
    /// <returns></returns>
    public static IIsleConfigurationBuilder ConfigureExtensionsLogging(this IIsleConfigurationBuilder @this, Action<IExtensionsLoggingConfigurationBuilder>? buildConfiguration = null)
    {
        var builder = new ConfigurationBuilder();
        @this.RegisterExtensionConfigurationHook(builder);
        buildConfiguration?.Invoke(builder);
        return @this;
    }

    private class ConfigurationBuilder : IExtensionsLoggingConfigurationBuilder, IIsleExtensionConfigurationHook
    {
        public bool EnableMessageTemplateCaching { get; set; } = true;
            
        public void ApplyExtensionConfiguration()
        {
            ExtensionsLoggingConfiguration.Current = new ExtensionsLoggingConfiguration
            {
                EnableMessageTemplateCaching = EnableMessageTemplateCaching
            };
        }

        public void ResetExtensionConfiguration()
        {
            ExtensionsLoggingConfiguration.Current = null!;
        }
    }
}