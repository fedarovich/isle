using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Isle.Extensions;

namespace Isle.Configuration;

/// <summary>
/// ISLE Configuration.
/// </summary>
public sealed class IsleConfiguration
{
    private static volatile IsleConfiguration? _current;

    private static readonly Func<string, string> DefaultValueNameConverter = static name => name;

    private readonly IList<IIsleExtensionConfigurationHook> _extensionHooks;
    private readonly Func<string, string>? _valueNameConverter;
    

    /// <summary>
    /// Gets the current ISLE configuration or throws <see cref="InvalidOperationException"/> if ISLE has not been configured yet.
    /// </summary>
    /// <exception cref="InvalidOperationException">ISLE has not been configured yet.</exception>
    /// <seealso cref="Configure"/>
    public static IsleConfiguration Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _current ?? ThrowInvalidOperationException();

            [DoesNotReturn]
            [MethodImpl(MethodImplOptions.NoInlining)]
            IsleConfiguration ThrowInvalidOperationException() => throw new InvalidOperationException(
                "Isle is not configured. Please, call IsleConfiguration.Configure before using Isle's logging methods.");
        }
    }

    private IsleConfiguration(IsleConfigurationBuilder builder)
    {
        ValueRepresentationPolicy = builder.ValueRepresentationPolicy ?? DefaultValueRepresentationPolicy.Instance;
        _valueNameConverter = builder.ValueNameConverter;
        PreserveDefaultValueRepresentationForExplicitNames = builder.PreserveDefaultValueRepresentationForExplicitNames;
        CacheLiteralValues = builder.CacheLiteralValues;
        _extensionHooks = builder.ExtensionHooks;
    }

    /// <summary>
    /// Gets the value representation policy.
    /// </summary>
    /// <remarks>
    /// <see cref="DefaultValueRepresentationPolicy"/> is used by default.
    /// </remarks>
    public IValueRepresentationPolicy ValueRepresentationPolicy { get; }

    /// <summary>
    /// Gets the value name converter.
    /// </summary>
    /// <remarks>
    /// Identity transform is used by default.
    /// </remarks>
    /// <seealso cref="ConvertValueName"/>
    [Obsolete($"This property is deprecated. Use {nameof(ConvertValueName)} method instead.")]
    public Func<string, string> ValueNameConverter => _valueNameConverter ?? DefaultValueNameConverter;

    /// <summary>
    /// Converts the value name using <see cref="ValueNameConverter"/> if one has been configured; otherwise returns the name unmodified.
    /// </summary>
    /// <param name="name">The arg</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ConvertValueName(string name) => _valueNameConverter == null ? name : _valueNameConverter(name);

    /// <summary>
    /// Gets the value indicating whether <see cref="LoggingExtensions.Named{T}(T,string)"/>
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
    public bool PreserveDefaultValueRepresentationForExplicitNames { get; }

    /// <summary>
    /// Gets the value indicating whether <see cref="LiteralValue"/>s can be cached by default.
    /// </summary>
    /// <remarks>
    /// CAUTION! Literal values may be cached only if they are compile-time or run-time constants.
    /// If literal value caching is enabled, passing non-constant values as <see cref="LiteralValue"/>s
    /// will cause memory leaks.
    /// </remarks>
    public bool CacheLiteralValues { get; }

    /// <summary>
    /// Configures ISLE.
    /// </summary>
    /// <param name="buildConfiguration">The action used for configuration.</param>
    /// <exception cref="ArgumentNullException"><paramref name="buildConfiguration"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">ISLE has already been configured.</exception>
    public static void Configure(Action<IIsleConfigurationBuilder> buildConfiguration)
    {
        if (buildConfiguration == null)
            throw new ArgumentNullException(nameof(buildConfiguration));

        var builder = new IsleConfigurationBuilder();
        buildConfiguration(builder);
        var configuration = new IsleConfiguration(builder);
        var prevConfiguration = Interlocked.CompareExchange(ref _current, configuration, null);
        if (prevConfiguration != null)
        {
            throw new InvalidOperationException("Isle has already been configured.");
        }

        foreach (var hook in configuration._extensionHooks)
        {
            hook.ApplyExtensionConfiguration();
        }
    }

    /// <summary>
    /// Resets the current configuration and calls the <see cref="IIsleExtensionConfigurationHook.ResetExtensionConfiguration"/>
    /// for all registered extensions.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is intended for unit testing and should never be used in production environment.
    /// </para>
    /// <para>
    /// If ISLE has not been configured yet or the configuration has been reset, this method will have no effect.
    /// </para>
    /// </remarks>
    public static void Reset()
    {
        var configuration = Interlocked.Exchange(ref _current, null);
        if (configuration != null)
        {
            foreach (var hook in configuration._extensionHooks.Reverse())
            {
                hook.ResetExtensionConfiguration();
            }
            configuration._extensionHooks.Clear();
        }
    }

    private class IsleConfigurationBuilder : IIsleConfigurationBuilder
    {
        public IValueRepresentationPolicy? ValueRepresentationPolicy { get; set; }
        
        public Func<string, string>? ValueNameConverter { get; set; }

        public IList<IIsleExtensionConfigurationHook> ExtensionHooks { get; } = new List<IIsleExtensionConfigurationHook>();

        public void RegisterExtensionConfigurationHook(IIsleExtensionConfigurationHook hook) => ExtensionHooks.Add(hook);
        
        public bool PreserveDefaultValueRepresentationForExplicitNames { get; set; }
        
        public bool CacheLiteralValues { get; set; }
    }
}