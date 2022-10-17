#if NETFRAMEWORK || NETSTANDARD2_0
// ReSharper disable CheckNamespace
namespace System.Diagnostics.CodeAnalysis;

/// <summary>Specifies that a method that will never return under any circumstance.</summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class DoesNotReturnAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
public sealed class NotNullAttribute : Attribute
{
}

#endif