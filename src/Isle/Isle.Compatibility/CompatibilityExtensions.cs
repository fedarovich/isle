using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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

#if NETFRAMEWORK || NETSTANDARD2_0

    public static bool StartsWith(this string s, char c)
    {
        return s.Length != 0 && s[0] == c;
    }

    public static StringBuilder Append(this StringBuilder @this, ReadOnlySpan<char> value)
    {
        if (value.Length > 0)
        {
            unsafe
            {
                fixed (char* valueChars = &MemoryMarshal.GetReference(value))
                {
                    @this.Append(valueChars, value.Length);
                }
            }
        }
        return @this;
    }

    public static TValue GetOrAdd<TKey, TValue, TArg>(
        this ConcurrentDictionary<TKey, TValue> @this,
        TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument
    )
    {
        return @this.GetOrAdd(key, k => valueFactory(k, factoryArgument));
    }
#endif
}