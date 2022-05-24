namespace Isle.Extensions.Logging;

internal sealed class FormattedLogValues : FormattedLogValuesBase
{
    private readonly KeyValuePair<string, object?>[] _values;

    public FormattedLogValues(int formattedCount)
    {
        _values = new KeyValuePair<string, object?>[formattedCount + 1];
    }

    internal override Span<KeyValuePair<string, object?>> Values => _values;

    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, object?>>)_values).GetEnumerator();
    }
}