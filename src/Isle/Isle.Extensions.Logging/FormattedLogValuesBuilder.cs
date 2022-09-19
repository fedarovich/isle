using System.Runtime.CompilerServices;
using Isle.Extensions.Logging.Configuration;

namespace Isle.Extensions.Logging;

internal abstract class FormattedLogValuesBuilder
{
    protected internal const string OriginalFormatName = "{OriginalFormat}";

    // We do not want to inline this method as it will add about additional 80 bytes of code each time we use logging.
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static FormattedLogValuesBuilder Acquire(int literalLength, int formattedCount)
    {
        bool enableCaching = ExtensionsLoggingConfiguration.Current.EnableMessageTemplateCaching;
        
        FormattedLogValuesBuilder builder = enableCaching
            ? CachingFormattedLogValuesBuilder.AcquireAndInitialize(formattedCount)
            : SimpleFormattedLogValuesBuilder.AcquireAndInitialize(literalLength, formattedCount);
        
        return builder;
    }

    public abstract FormattedLogValuesBase BuildAndReset();

    public abstract void AppendLiteral(string str);

    public abstract void AppendLiteralValue(in LiteralValue literalValue);

    public abstract void AppendFormatted(string name, object? value);

    public abstract void AppendFormatted(string name, object? value, int alignment, string? format);
}