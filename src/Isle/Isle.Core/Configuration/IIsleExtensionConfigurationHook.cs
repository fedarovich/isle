namespace Isle.Configuration;

/// <summary>
/// Provides methods to call after the configuration is applied or reset.
/// </summary>
/// <remarks>
/// ISLE integrations, such as Isle.Extensions.Logging and Isle.Serilog, register their hooks with
/// <see cref="IIsleConfigurationBuilder.RegisterExtensionConfigurationHook"/> to apply their own configuration.
/// </remarks>
public interface IIsleExtensionConfigurationHook
{
    /// <summary>
    /// Callback method that is called by <see cref="IsleConfiguration.Configure"/> after the core ISLE configuration has been applied.
    /// </summary>
    /// <remarks>
    /// The hooks are called in the order they were registered.
    /// </remarks>
    void ApplyExtensionConfiguration();

    /// <summary>
    /// Callback method that is called by <see cref="IsleConfiguration.Reset"/> when the ISLE configuration is reset.
    /// </summary>
    /// <remarks>
    /// The hooks are called in the reverse order of registration.
    /// </remarks>
    void ResetExtensionConfiguration()
#if NETCOREAPP
    {
    }
#else
    ;
#endif
}