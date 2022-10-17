#if !NET6_0_OR_GREATER
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Isle;

internal static class SpanFormattableHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsSpanFormattable<T>(T value)
    {
#if NETCOREAPP || NETSTANDARD2_1
        return typeof(T) == typeof(byte)
               || typeof(T) == typeof(sbyte)
               || typeof(T) == typeof(short)
               || typeof(T) == typeof(ushort)
               || typeof(T) == typeof(int)
               || typeof(T) == typeof(uint)
               || typeof(T) == typeof(long)
               || typeof(T) == typeof(ulong)
               || typeof(T) == typeof(float)
               || typeof(T) == typeof(double)
               || typeof(T) == typeof(decimal)
               || typeof(T) == typeof(BigInteger)
               || typeof(T) == typeof(DateTime)
               || typeof(T) == typeof(DateTimeOffset)
               || typeof(T) == typeof(TimeSpan)
               || typeof(T) == typeof(Guid);
#else
        return false;
#endif
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryFormat<T>(
        T value, 
        Span<char> destination, 
        out int charsWritten,
#if NETCOREAPP || NETSTANDARD2_1
        ReadOnlySpan<char> format,
#else
        string? format,
#endif
        IFormatProvider? provider)
    {
#if NETCOREAPP || NETSTANDARD2_1
        return value switch
        {
            byte v => v.TryFormat(destination, out charsWritten, format, provider),
            sbyte v => v.TryFormat(destination, out charsWritten, format, provider),
            short v => v.TryFormat(destination, out charsWritten, format, provider),
            ushort v => v.TryFormat(destination, out charsWritten, format, provider),
            int v => v.TryFormat(destination, out charsWritten, format, provider),
            uint v => v.TryFormat(destination, out charsWritten, format, provider),
            long v => v.TryFormat(destination, out charsWritten, format, provider),
            ulong v => v.TryFormat(destination, out charsWritten, format, provider),
            float v => v.TryFormat(destination, out charsWritten, format, provider),
            double v => v.TryFormat(destination, out charsWritten, format, provider),
            decimal v => v.TryFormat(destination, out charsWritten, format, provider),
            BigInteger v => v.TryFormat(destination, out charsWritten, format, provider),
            DateTime v => v.TryFormat(destination, out charsWritten, format, provider),
            DateTimeOffset v => v.TryFormat(destination, out charsWritten, format, provider),
            TimeSpan v => v.TryFormat(destination, out charsWritten, format, provider),
            Guid v => v.TryFormat(destination, out charsWritten, format),
            _ => Fallback(out charsWritten)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Fallback(out int charsWritten)
        {
            charsWritten = 0;
            return false;
        }
#else
        charsWritten = 0;
        return false;
#endif
    }
}
#endif