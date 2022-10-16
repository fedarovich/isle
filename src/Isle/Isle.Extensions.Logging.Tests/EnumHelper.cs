using System;

namespace Isle.Extensions.Logging.Tests;

internal static class EnumHelper
{
    internal static T[] GetValues<T>() where T : struct, Enum
    {
#if NET5_0_OR_GREATER
        return Enum.GetValues<T>();
#else
        return (T[]) Enum.GetValues(typeof(T));
#endif
    }
}