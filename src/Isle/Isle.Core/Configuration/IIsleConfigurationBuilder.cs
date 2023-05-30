using Isle.Extensions;

namespace Isle.Configuration;

/// <summary>
/// Provides the properties and methods to configure ISLE.
/// </summary>
public interface IIsleConfigurationBuilder
{
    /// <summary>
    /// Gets or sets the value representation policy.
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/>, <see cref="DefaultValueRepresentationPolicy"/> will be used.
    /// </remarks>
    IValueRepresentationPolicy? ValueRepresentationPolicy { get; set; }

    /// <summary>
    /// Gets or sets the value name converter.
    /// </summary>
    /// <remarks>
    /// If <see langword="null"/>, a function returning original value will be used.
    /// </remarks>
    Func<string, string>? ValueNameConverter { get; set; }

    /// <summary>
    /// Registers an extension configuration hook.
    /// </summary>
    /// <param name="hook">The hook to register.</param>
    void RegisterExtensionConfigurationHook(IIsleExtensionConfigurationHook hook);

    /// <summary>
    /// Gets or sets the value indicating whether <see cref="LoggingExtensions.Named{T}(T,string)"/>
    /// will preserve the default value representation.
    /// </summary>
    /// <value>
    /// <para>
    /// If <see langword="true"/>, the method <see cref="LoggingExtensions.Named{T}(T,string)"/> will use the name as it is;
    /// otherwise, depending on the <see cref="ValueRepresentationPolicy"/>,
    /// the name can be prepended with <c>@</c> for destructuring or with <c>$</c> for stringification.
    /// </para>
    /// <para>
    /// The default value is <see langword="false" />.
    /// </para>
    /// </value>
    bool PreserveDefaultValueRepresentationForExplicitNames { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether <see cref="LiteralValue"/>s can be cached by default.
    /// </summary>
    /// <remarks>
    /// CAUTION! Literal values may be cached only if they are compile-time or run-time constants.
    /// If literal value caching is enabled, passing non-constant values as <see cref="LiteralValue"/>s
    /// will cause memory leaks.
    /// </remarks>
    /// <value>
    /// <para>The value indicating whether <see cref="LiteralValue"/>s can be cached by default.</para>
    /// <para>The default value is <see langword="false" />.</para>
    /// </value>
    bool CacheLiteralValues { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the ISLE configuration can be reset.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is not recommended to mark the configuration as resettable, as it will disable certain optimizations.
    /// </para>
    /// <para>
    /// Resettable configurations are mainly intended to be used with unit tests.
    /// </para>
    /// </remarks>
    bool IsResettable { get; set; }
}