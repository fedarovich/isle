using System.Runtime.CompilerServices;
using Isle.Extensions.Logging.Configuration;

namespace Isle.Extensions.Logging;

internal abstract class FormattedLogValuesBuilder
{
    protected const string OriginalFormatName = "{OriginalFormat}";

    [ThreadStatic]
    private static FormattedLogValuesBuilder? _cachedInstance;

    public static FormattedLogValuesBuilder Acquire(int literalLength, int formattedCount)
    {
        bool enableCaching = ExtensionsLoggingConfiguration.Current.EnableMessageTemplateCaching;
        var builder = _cachedInstance;
        if (builder != null)
        {
            _cachedInstance = null;
            if (enableCaching != builder.IsCaching)
            {
                builder = CreateFormattedLogValuesBuilder(enableCaching);
            }
        }
        else
        {
            builder = CreateFormattedLogValuesBuilder(enableCaching);
        }
        
        builder.Initialize(literalLength, formattedCount);
        return builder;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static FormattedLogValuesBuilder CreateFormattedLogValuesBuilder(bool enableCaching) =>
            enableCaching ? new CachingFormattedLogValuesBuilder() : new SimpleFormattedLogValuesBuilder();
    }

    public static FormattedLogValuesBase BuildAndRelease(FormattedLogValuesBuilder builder)
    {
        var result = builder.BuildAndReset();
        _cachedInstance = builder;
        return result;
    }

    public abstract bool IsCaching { get; }

    protected abstract void Initialize(int literalLength, int formattedCount);

    protected abstract FormattedLogValuesBase BuildAndReset();

    public abstract void AppendLiteral(string? str);

    public abstract void AppendFormatted(string name, object? value, int alignment = 0, string? format = null);
}