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
    /// Adds and configures the ISLE extension for integration with <see cref="ILogger"/>.
    /// </summary>
    /// <param name="this">The ISLE configuration builder.</param>
    /// <param name="buildConfiguration">An optional action to configure the integration.</param>
    /// <returns>The ISLE configuration builder, for chaining.</returns>
    /// <remarks>
    /// The integration must be added before the Isle.Serilog logging methods can be used. Add it only once.
    /// </remarks>
    public static IIsleConfigurationBuilder AddSerilog(this IIsleConfigurationBuilder @this,
        Action<ISerilogConfigurationBuilder>? buildConfiguration = null)
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
    /// <remarks>This method is obsolete. Use <see cref="AddSerilog"/> instead.</remarks>
    /// <seealso cref="AddSerilog" />
    [Obsolete($"Use {nameof(AddSerilog)} method instead."
#if NETCOREAPP
        , DiagnosticId = "ISLE2000"
#endif
    )]
    public static IIsleConfigurationBuilder ConfigureSerilog(this IIsleConfigurationBuilder @this, Action<ISerilogConfigurationBuilder>? buildConfiguration = null)
    {
        return @this.AddSerilog(buildConfiguration);
    }

    private class ConfigurationBuilder : ISerilogConfigurationBuilder, IIsleExtensionConfigurationHook
    {
        public bool EnableMessageTemplateCaching { get; set; } = true;

        public void ApplyExtensionConfiguration()
        {
            SerilogConfigurationSnapshot.Current = new (this);
        }

        public void ResetExtensionConfiguration()
        {
            SerilogConfigurationSnapshot.Reset();
        }
    }
}