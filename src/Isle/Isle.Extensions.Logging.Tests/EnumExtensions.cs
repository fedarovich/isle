#if !NET5_0_OR_GREATER
using System;

namespace Isle.Extensions.Logging.Tests;

internal static class EnumExtensions
{
    extension(Enum)
    {
        internal static T[] GetValues<T>() where T : struct, Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
#endif