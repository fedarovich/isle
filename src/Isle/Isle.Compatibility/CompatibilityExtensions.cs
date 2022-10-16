using System;
using System.Runtime.CompilerServices;

namespace Isle;

internal static class CompatibilityExtensions
{
    public static ref char GetRawStringData(this string s)
    {
#if NETCOREAPP
        ref readonly char @ref = ref s.GetPinnableReference();
#else
        ref readonly char @ref = ref s.AsSpan().GetPinnableReference();
#endif
        return ref Unsafe.AsRef(in @ref);
    }
}