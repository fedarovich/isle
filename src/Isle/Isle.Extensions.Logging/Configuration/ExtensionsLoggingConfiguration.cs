using System.Runtime.CompilerServices;

namespace Isle.Extensions.Logging.Configuration;

internal static class ExtensionsLoggingConfiguration
{
    internal static readonly bool IsResettable;

    private static readonly bool _enableMessageTemplateCaching;

    static ExtensionsLoggingConfiguration()
    {
        if (ExtensionsLoggingConfigurationSnapshot.TryGetCurrent(out var snapshot) && !snapshot.IsResettable)
        {
            IsResettable = false;
            _enableMessageTemplateCaching = snapshot.EnableMessageTemplateCaching;
        }
        else
        {
            IsResettable = true;
        }
    }

    public static bool EnableMessageTemplateCaching
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsResettable ? ExtensionsLoggingConfigurationSnapshot.Current.EnableMessageTemplateCaching : _enableMessageTemplateCaching;
    }
}