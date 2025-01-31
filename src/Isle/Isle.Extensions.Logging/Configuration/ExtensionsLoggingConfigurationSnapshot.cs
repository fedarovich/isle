using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Isle.Configuration;

namespace Isle.Extensions.Logging.Configuration;

internal class ExtensionsLoggingConfigurationSnapshot
{
    private static volatile ExtensionsLoggingConfigurationSnapshot? _current;

    public ExtensionsLoggingConfigurationSnapshot(IExtensionsLoggingConfigurationBuilder builder)
    {
        EnableMessageTemplateCaching = builder.EnableMessageTemplateCaching;
        IsResettable = CoreConfigurationSnapshot.Current.IsResettable;
    }

    public static ExtensionsLoggingConfigurationSnapshot Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _current ?? ThrowInvalidOperationException();

            [DoesNotReturn]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static ExtensionsLoggingConfigurationSnapshot ThrowInvalidOperationException() => throw new InvalidOperationException(
                "Isle.Extension.Logging is not configured. Please, call IIsleConfigurationBuilder.AddExtensionsLogging() inside IsleConfiguration.Configure.");
        }
        set
        {
            var previous = Interlocked.CompareExchange(ref _current, value, null);
            if (previous != null)
            {
                throw new InvalidOperationException("Isle.Extension.Logging has already been configured.");
            }
        }
    }

    internal static bool TryGetCurrent([NotNullWhen(true)] out ExtensionsLoggingConfigurationSnapshot? current)
    {
        current = _current;
        return current != null;
    }

    internal static void Reset()
    {
        if (_current == null)
            return;

        if (!_current.IsResettable)
            throw new InvalidOperationException("The ISLE configuration is not resettable.");

        _current = null;
    }

    internal bool EnableMessageTemplateCaching { get; }

    internal bool IsResettable { get; }
}