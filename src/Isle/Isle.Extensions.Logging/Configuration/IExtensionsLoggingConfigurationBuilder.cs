using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging.Configuration;

/// <summary>
/// Provides the properties and methods to configure ISLE extensions for <see cref="ILogger"/>.
/// </summary>
public interface IExtensionsLoggingConfigurationBuilder
{
    /// <summary>
    /// Gets or sets the value indicating whether the message templates are cached.
    /// </summary>
    /// <remarks>
    /// This property is <see langword="true"/> by default.
    /// </remarks>
    bool EnableMessageTemplateCaching { get; set; }
}