using Isle.Extensions.Logging.Caching;

namespace Isle.Extensions.Logging;

internal class CachingFormattedLogValuesBuilder : FormattedLogValuesBuilder
{
    private Node _lastNode = null!;
    private KeyValuePair<string, object?>[] _formattedLogValues = null!;
    private int _valueIndex = 0;

    protected override void Initialize(int literalLength, int formattedCount)
    {
        _lastNode = NodeCache.Instance;
        _formattedLogValues = new KeyValuePair<string, object?>[formattedCount + 1];
    }

    protected override FormattedLogValues BuildAndReset()
    {
        var templateNode = _lastNode.GetTemplateNode();
        _formattedLogValues[_valueIndex] = new(OriginalFormatName, templateNode.MessageTemplate);
        var formattedLogValues = new FormattedLogValues(_formattedLogValues, templateNode.Segments, templateNode.Segments.Length);

        _lastNode = null!;
        _formattedLogValues = null!;
        _valueIndex = 0;

        return formattedLogValues;
    }

    public override void AppendLiteral(string? str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            _lastNode = _lastNode.GetOrAddLiteralNode(str);
        }
    }

    public override void AppendFormatted(string name, object? value, int alignment = 0, string? format = null)
    {
        var formatNode = _lastNode.GetOrAddFormatNode(name, alignment, format);
        _lastNode = formatNode;
        _formattedLogValues[_valueIndex++] = new(formatNode.Name, value);
    }
}