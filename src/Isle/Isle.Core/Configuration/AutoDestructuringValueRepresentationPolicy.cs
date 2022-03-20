using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isle.Configuration;

public sealed class AutoDestructuringValueRepresentationPolicy : IValueRepresentationPolicy
{
    public static AutoDestructuringValueRepresentationPolicy Instance
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] get;
    }

    static AutoDestructuringValueRepresentationPolicy()
    {
        Instance = new AutoDestructuringValueRepresentationPolicy();
    }

    private AutoDestructuringValueRepresentationPolicy()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueRepresentation GetRepresentationOfType<T>() =>
        TypeInfo<T>.IsScalar ? ValueRepresentation.Default : ValueRepresentation.Destructure;

    private static class TypeInfo<T>
    {
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        public static readonly bool IsScalar;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        static TypeInfo()
        {
            var type = typeof(T);
            type = Nullable.GetUnderlyingType(type) ?? type;
            IsScalar = type.IsPrimitive
                    || type.IsEnum
                    || type == typeof(string)
                    || type == typeof(decimal)
                    || type == typeof(DateTime)
                    || type == typeof(DateTimeOffset)
                    || type == typeof(TimeSpan)
                    || type == typeof(Uri)
                    || type == typeof(Guid);
        }
    }

    
}