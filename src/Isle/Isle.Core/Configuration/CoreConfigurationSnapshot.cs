using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isle.Configuration;

internal class CoreConfigurationSnapshot
{
    private static volatile CoreConfigurationSnapshot? _current;

    internal static CoreConfigurationSnapshot Current
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _current ?? ThrowInvalidOperationException();

            [DoesNotReturn]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static CoreConfigurationSnapshot ThrowInvalidOperationException() => throw new InvalidOperationException(
                "Isle is not configured. Please, call IsleConfiguration.Configure before using Isle's logging methods.");
        }
        set
        {
            var previous = Interlocked.CompareExchange(ref _current, value, null);
            if (previous != null)
            {
                throw new InvalidOperationException("Isle has already been configured.");
            }
        }
    }

    internal static bool TryGetCurrent([NotNullWhen(true)] out CoreConfigurationSnapshot? current)
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

    public CoreConfigurationSnapshot(IIsleConfigurationBuilder builder)
    {
        ValueRepresentationPolicy = builder.ValueRepresentationPolicy ?? DefaultValueRepresentationPolicy.Instance;
        ValueNameConverter = builder.ValueNameConverter;
        PreserveDefaultValueRepresentationForExplicitNames = builder.PreserveDefaultValueRepresentationForExplicitNames;
        CacheLiteralValues = builder.CacheLiteralValues;
        IsResettable = builder.IsResettable;
    }

    internal IValueRepresentationPolicy ValueRepresentationPolicy { get; }

    public Func<string, string>? ValueNameConverter { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal string ConvertValueName(string name) => ValueNameConverter == null ? name : ValueNameConverter(name);
    
    internal bool PreserveDefaultValueRepresentationForExplicitNames { get; }

    internal bool CacheLiteralValues { get; }

    internal bool IsResettable { get; }
}