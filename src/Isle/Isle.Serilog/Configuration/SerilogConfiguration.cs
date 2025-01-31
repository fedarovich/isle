using System.Runtime.CompilerServices;

namespace Isle.Serilog.Configuration;

internal class SerilogConfiguration
{
    internal static readonly bool IsResettable;

    private static readonly bool _enableMessageTemplateCaching;

    static SerilogConfiguration()
    {
        if (SerilogConfigurationSnapshot.TryGetCurrent(out var snapshot) && !snapshot.IsResettable)
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
        get => IsResettable ? SerilogConfigurationSnapshot.Current.EnableMessageTemplateCaching : _enableMessageTemplateCaching;
    }
}