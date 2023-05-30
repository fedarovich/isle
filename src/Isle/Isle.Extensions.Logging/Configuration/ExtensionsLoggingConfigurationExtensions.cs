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
    /// Adds and configures the ISLE extension for integration with <see cref="ILogger"/>.
    /// </summary>
    public static IIsleConfigurationBuilder AddExtensionsLogging(this IIsleConfigurationBuilder @this, 
        Action<IExtensionsLoggingConfigurationBuilder>? buildConfiguration = null)
    {
        var builder = new ConfigurationBuilder();
        @this.RegisterExtensionConfigurationHook(builder);
        buildConfiguration?.Invoke(builder);
        return @this;
    }

    /// <summary>
    /// Adds and configures the ISLE extension for integration with <see cref="ILogger"/>.
    /// </summary>
    /// <seealso cref="AddExtensionsLogging"/>
    [Obsolete($"Use {nameof(AddExtensionsLogging)} method instead.")]
    public static IIsleConfigurationBuilder ConfigureExtensionsLogging(this IIsleConfigurationBuilder @this,
        Action<IExtensionsLoggingConfigurationBuilder>? buildConfiguration = null)
    {
        return @this.AddExtensionsLogging(buildConfiguration);
    }
    
    private class ConfigurationBuilder : IExtensionsLoggingConfigurationBuilder, IIsleExtensionConfigurationHook
    {
        public bool EnableMessageTemplateCaching { get; set; } = true;
            
        public void ApplyExtensionConfiguration()
        {
            ExtensionsLoggingConfigurationSnapshot.Current = new (this);
        }

        public void ResetExtensionConfiguration()
        {
            ExtensionsLoggingConfigurationSnapshot.Reset();
        }
    }
}