using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Isle.Configuration;

namespace Isle.Serilog.Configuration;

internal class SerilogConfigurationSnapshot
{
    private static volatile SerilogConfigurationSnapshot? _current;

    public SerilogConfigurationSnapshot(ISerilogConfigurationBuilder builder)
    {
        EnableMessageTemplateCaching = builder.EnableMessageTemplateCaching;
        IsResettable = CoreConfigurationSnapshot.Current.IsResettable;
    }

    public static SerilogConfigurationSnapshot Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _current ?? ThrowInvalidOperationException();

            [DoesNotReturn]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static SerilogConfigurationSnapshot ThrowInvalidOperationException() => throw new InvalidOperationException(
                "Isle.Serilog is not configured. Please, call IIsleConfigurationBuilder.AddSerilog() inside IsleConfiguration.Configure.");
        }
        set
        {
            var previous = Interlocked.CompareExchange(ref _current, value, null);
            if (previous != null)
            {
                throw new InvalidOperationException("Isle.Serilog has already been configured.");
            }
        }
    }

    internal static bool TryGetCurrent([NotNullWhen(true)] out SerilogConfigurationSnapshot? current)
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
