using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isle.Configuration;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class CoreConfiguration
{
    internal static readonly bool IsResettable;

    private static readonly IValueRepresentationPolicy _valueRepresentationPolicy = null!;
    private static readonly Func<string, string>? _valueNameConverter;
    private static readonly bool _preserveDefaultValueRepresentationForExplicitNames;
    private static readonly bool _cacheLiteralValues;

    static CoreConfiguration()
    {
        if (CoreConfigurationSnapshot.TryGetCurrent(out var snapshot) && !snapshot.IsResettable)
        {
            IsResettable = false;
            _valueRepresentationPolicy = snapshot.ValueRepresentationPolicy;
            _valueNameConverter = snapshot.ValueNameConverter;
            _preserveDefaultValueRepresentationForExplicitNames = snapshot.PreserveDefaultValueRepresentationForExplicitNames;
            _cacheLiteralValues = snapshot.CacheLiteralValues;
        }
        else
        {
            IsResettable = true;
        }
    }

    internal static IValueRepresentationPolicy ValueRepresentationPolicy
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsResettable ? CoreConfigurationSnapshot.Current.ValueRepresentationPolicy : _valueRepresentationPolicy;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [SuppressMessage("ReSharper", "OperatorIsCanBeUsed")]
    internal static ValueRepresentation GetRepresentationOfType<T>()
    {
        if (IsResettable)
        {
            return ValueRepresentationPolicy.GetRepresentationOfType<T>();
        }

        if (_valueRepresentationPolicy.GetType() == typeof(DefaultValueRepresentationPolicy))
            return ValueRepresentation.Default;

        if (_valueRepresentationPolicy.GetType() == typeof(AutoDestructuringValueRepresentationPolicy))
            return AutoDestructuringValueRepresentationPolicy.TypeInfo<T>.IsScalar ? ValueRepresentation.Default : ValueRepresentation.Destructure;

        return HelperGetRepresentationOfType();

        // This helper method is needed for correct inlining.
        // Calling _valueRepresentationPolicy.GetRepresentationOfType<T>() directly prevents dead code elimination for some reason.
        static ValueRepresentation HelperGetRepresentationOfType() => _valueRepresentationPolicy.GetRepresentationOfType<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string ConvertValueName(string name)
    {
        if (IsResettable)
        {
            return CoreConfigurationSnapshot.Current.ConvertValueName(name);
        }

        return _valueNameConverter != null ? _valueNameConverter(name) : name;
    }

    internal static bool PreserveDefaultValueRepresentationForExplicitNames
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsResettable
            ? CoreConfigurationSnapshot.Current.PreserveDefaultValueRepresentationForExplicitNames
            : _preserveDefaultValueRepresentationForExplicitNames;
    }

    internal static bool CacheLiteralValues
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsResettable
            ? CoreConfigurationSnapshot.Current.CacheLiteralValues
            : _cacheLiteralValues;
    }
}