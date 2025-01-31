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
    public static IIsleConfigurationBuilder AddSerilog(this IIsleConfigurationBuilder @this,
        Action<ISerilogConfigurationBuilder>? buildConfiguration = null)
    {
        var builder = new ConfigurationBuilder();
        @this.RegisterExtensionConfigurationHook(builder);
        buildConfiguration?.Invoke(builder);
        return @this;
    }

    /// <summary>
    /// Adds and configures the parameters for integration with <see cref="ILogger"/>.
    /// </summary>
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