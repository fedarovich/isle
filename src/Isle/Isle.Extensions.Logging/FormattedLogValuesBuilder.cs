using System.Runtime.CompilerServices;
using Isle.Extensions.Logging.Configuration;

namespace Isle.Extensions.Logging;

internal static class FormattedLogValuesBuilder
{
    internal const string OriginalFormatName = "{OriginalFormat}";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IFormattedLogValuesBuilder Acquire(int literalLength, int formattedCount)
    {
        var formattedLogValues = FormattedLogValuesBase.Create(formattedCount);
        
        IFormattedLogValuesBuilder builder = ExtensionsLoggingConfiguration.EnableMessageTemplateCaching
            ? CachingFormattedLogValuesBuilder.AcquireAndInitialize(formattedLogValues)
            : SimpleFormattedLogValuesBuilder.AcquireAndInitialize(literalLength, formattedCount, formattedLogValues);
        
        return builder;
    }
}