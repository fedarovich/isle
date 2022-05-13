using Isle.Configuration;

namespace Isle.Extensions.Logging;

internal abstract class FormattedLogValuesBuilder
{
    protected const string OriginalFormatName = "{OriginalFormat}";
    protected const char DestructureOperator = '@';
    protected const char StringifyOperator = '$';

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

    public abstract void AppendFormatted(in NamedLogValue namedLogValue, int alignment = 0, string? format = null);

    internal static string TransformName(in NamedLogValue namedLogValue)
    {
        var name = namedLogValue.HasExplicitName
            ? namedLogValue.Name
            : IsleConfiguration.Current.ValueNameConverter(namedLogValue.Name);
        if (!name.StartsWith(DestructureOperator) && !name.StartsWith(StringifyOperator))
        {
            switch (namedLogValue.Representation)
            {
                case ValueRepresentation.Destructure:
                    name = DestructureOperator + name;
                    break;
                case ValueRepresentation.Stringify:
                    name = StringifyOperator + name;
                    break;
            }
        }

        return name;
    }
}