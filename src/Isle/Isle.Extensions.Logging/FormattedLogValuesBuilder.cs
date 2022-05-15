namespace Isle.Extensions.Logging;

internal abstract class FormattedLogValuesBuilder
{
    protected const string OriginalFormatName = "{OriginalFormat}";

    [ThreadStatic]
    private static FormattedLogValuesBuilder? _cachedInstance;

    public static FormattedLogValuesBuilder Acquire(int literalLength, int formattedCount)
    {
        var builder = _cachedInstance;
        if (builder != null)
        {
            _cachedInstance = null;
        }
        else
        {
            builder = new CachingFormattedLogValuesBuilder();
        }

        builder.Initialize(literalLength, formattedCount);
        return builder;
    }

    public static FormattedLogValues BuildAndRelease(FormattedLogValuesBuilder builder)
    {
        var result = builder.BuildAndReset();
        _cachedInstance = builder;
        return result;
    }

    protected abstract void Initialize(int literalLength, int formattedCount);

    protected abstract FormattedLogValues BuildAndReset();

    public abstract void AppendLiteral(string? str);

    public abstract void AppendFormatted(string name, object? value, int alignment = 0, string? format = null);
}