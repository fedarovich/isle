namespace Isle.Configuration;

/// <summary>
/// Provides methods to call after the configuration is applied or reset.
/// </summary>
public interface IIsleExtensionConfigurationHook
{
    /// <summary>
    /// Callback method that is called after the ISLE is configured is applied.
    /// </summary>
    void ApplyExtensionConfiguration();

    /// <summary>
    /// Callback method that is called after the ISLE configuration is reset.
    /// </summary>
    void ResetExtensionConfiguration()
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1
    {
    }
#else
    ;
#endif
}