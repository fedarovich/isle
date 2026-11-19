using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Isle;

internal static class CompatibilityExtensions
{
#if NETFRAMEWORK || NETSTANDARD2_0

    extension(string s)
    {
        public ref char GetRawStringData()
        {
            ref readonly char @ref = ref s.AsSpan().GetPinnableReference();
            return ref Unsafe.AsRef(in @ref);
        }

        public bool StartsWith(char c)
        {
            return s.Length != 0 && s[0] == c;
        }
    }

    extension(StringBuilder @this)
    {
        public StringBuilder Append(ReadOnlySpan<char> value)
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
    }

    extension<TKey, TValue>(ConcurrentDictionary<TKey, TValue> @this)
    {
        public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument)
        {
            return @this.GetOrAdd(key, k => valueFactory(k, factoryArgument));
        }
    }

    extension(ArgumentNullException)
    {
        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }

            [DoesNotReturn]
            static void Throw(string? paramName) =>
                throw new ArgumentNullException(paramName);
        }
    }

#endif
}