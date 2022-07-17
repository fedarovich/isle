namespace Isle.Extensions.Logging;

internal sealed class FormattedLogValues : FormattedLogValuesBase
{
    private readonly KeyValuePair<string, object?>[] _values;

    public FormattedLogValues(int formattedCount) : base(formattedCount + 1)
    {
        _values = new KeyValuePair<string, object?>[Count];
    }

    internal override Span<KeyValuePair<string, object?>> Values => _values.AsSpan(0, Count);

    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == _values.Length 
            ? ((IEnumerable<KeyValuePair<string, object?>>)_values).GetEnumerator()
            : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return _values[i];
            }
        }
    }
}