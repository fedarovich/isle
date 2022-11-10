using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isle.Configuration;

#pragma warning disable CS1574, CS1584, CS1581, CS1580
/// <summary>
/// <see cref="IValueRepresentationPolicy"/> that sets the representation of non-scalar types to <see cref="ValueRepresentation.Destructure"/>.
/// </summary>
/// <remarks>
/// <para>
/// The values of the scalar types are represented in their default form. The following type are treated as scalar:
/// </para>
/// <list type="bullet">
///     <item>
///         <description>
///             All integral types: <see cref="byte"/>, <see cref="sbyte"/>, <see cref="short"/>, <see cref="ushort"/>,
///             <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <see cref="IntPtr"/>, <see cref="UIntPtr"/>
///         </description>
///     </item>
///     <item>
///         <description>All floating point types: <see cref="float"/>, <see cref="double"/>.</description>
///     </item>
///     <item>
///         <description><see cref="decimal"/></description>
///     </item>
///     <item>
///         <description><see cref="char"/></description>
///     </item>
///     <item>
///         <description><see cref="bool"/></description>
///     </item>
///     <item>
///         <description><see cref="string"/></description>
///     </item>
///     <item>
///         <description>All enumeration types.</description>
///     </item>
///     <item>
///         <description><see cref="DateTime"/></description>
///     </item>
///     <item>
///         <description><see cref="DateTimeOffset"/></description>
///     </item>
///     <item>
///         <description><see cref="TimeSpan"/></description>
///     </item>
///     <item>
///         <description><see cref="DateOnly"/></description>
///     </item>
///     <item>
///         <description><see cref="TimeOnly"/></description>
///     </item>
///     <item>
///         <description><see cref="Guid"/></description>
///     </item>
///     <item>
///         <description><see cref="Uri"/></description>
///     </item>
/// </list>
/// <para>
/// All other types are treated as complex and their values will be destructured.
/// </para>
/// </remarks>
#pragma warning restore CS1574, CS1584, CS1581, CS1580
public sealed class AutoDestructuringValueRepresentationPolicy : IValueRepresentationPolicy
{
    /// <summary>
    /// Gets the singleton instance of the policy.
    /// </summary>
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

    /// <inheritdoc cref="IValueRepresentationPolicy.GetRepresentationOfType{T}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ValueRepresentation GetRepresentationOfType<T>() =>
        TypeInfo<T>.IsScalar ? ValueRepresentation.Default : ValueRepresentation.Destructure;

    private static class TypeInfo<T>
    {
        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        public static readonly bool IsScalar;

#if NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
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
#if NET6_0_OR_GREATER
                    || type == typeof(DateOnly)
                    || type == typeof(TimeOnly)
#endif
#if NET7_0_OR_GREATER
                    || type == typeof(Int128)
                    || type == typeof(UInt128)
#endif
                    || type == typeof(Uri)
                    || type == typeof(Guid);
        }
    }

    
}