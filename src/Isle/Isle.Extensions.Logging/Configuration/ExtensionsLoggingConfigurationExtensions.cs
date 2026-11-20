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
    /// <param name="this">The ISLE configuration builder.</param>
    /// <param name="buildConfiguration">An optional action to configure the integration.</param>
    /// <returns>The ISLE configuration builder, for chaining.</returns>
    /// <remarks>
    /// The integration must be added before the Isle.Extensions.Logging logging methods can be used. Add it only once.
    /// </remarks>
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
    /// <param name="this">The ISLE configuration builder.</param>
    /// <param name="buildConfiguration">An optional action to configure the integration.</param>
    /// <returns>The ISLE configuration builder, for chaining.</returns>
    /// <remarks>This method is obsolete. Use <see cref="AddExtensionsLogging"/> instead.</remarks>
    /// <seealso cref="AddExtensionsLogging"/>
    [Obsolete($"Use {nameof(AddExtensionsLogging)} method instead."
#if NETCOREAPP
        , DiagnosticId = "ISLE1000"
#endif
        )]
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