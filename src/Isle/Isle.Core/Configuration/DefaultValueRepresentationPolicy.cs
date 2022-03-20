using System.Runtime.CompilerServices;

namespace Isle.Configuration;

public sealed class DefaultValueRepresentationPolicy : IValueRepresentationPolicy
{
    public static DefaultValueRepresentationPolicy Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    static DefaultValueRepresentationPolicy()
    {
        Instance = new DefaultValueRepresentationPolicy();
    }

    private DefaultValueRepresentationPolicy()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueRepresentation GetRepresentationOfType<T>() => ValueRepresentation.Default;
}