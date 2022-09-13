using Isle.Extensions.Logging.Caching;

namespace Isle.Extensions.Logging;

internal sealed class CachingFormattedLogValuesBuilder : FormattedLogValuesBuilder
{
    private Node _lastNode = null!;
    private FormattedLogValuesBase _formattedLogValues = null!;
    private int _valueIndex = 0;

    public override bool IsCaching => true;

    protected override void Initialize(int literalLength, int formattedCount)
    {
        _lastNode = NodeCache.Instance;
        _formattedLogValues = FormattedLogValuesBase.Create(formattedCount);
    }

    protected override FormattedLogValuesBase BuildAndReset()
    {
        var templateNode = _lastNode.GetTemplateNode();
        var result = _formattedLogValues;
        result.Values[_valueIndex] = new(OriginalFormatName, templateNode.MessageTemplate);
        result.Count = _valueIndex + 1;
        result.SetSegments(templateNode.Segments);
        
        _lastNode = null!;
        _formattedLogValues = null!;
        _valueIndex = 0;

        return result;
    }

    public override void AppendLiteral(string? str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            _lastNode = _lastNode.GetOrAddLiteralNode(str);
        }
    }

    public override void AppendLiteralValue(in LiteralValue literalValue)
    {
        if (literalValue.IsCacheable)
        {
            AppendLiteral(literalValue.Value);
        }
        else if (!string.IsNullOrEmpty(literalValue.Value))
        {
            _lastNode = _lastNode.CreateNotCachedLiteralNode(literalValue.Value);
        }
    }

    public override void AppendFormatted(string name, object? value, int alignment = 0, string? format = null)
    {
        _lastNode = (alignment == 0 && string.IsNullOrEmpty(format))
            ? _lastNode.GetOrAddHoleNode(name)
            : _lastNode.GetOrAddFormattedHoleNode(name, alignment, format);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }
}