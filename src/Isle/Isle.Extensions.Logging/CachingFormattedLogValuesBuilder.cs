using System.Runtime.CompilerServices;
using Isle.Extensions.Logging.Caching;

namespace Isle.Extensions.Logging;

internal sealed class CachingFormattedLogValuesBuilder : FormattedLogValuesBuilder
{
    [ThreadStatic]
    private static CachingFormattedLogValuesBuilder? _cachedInstance;

    private Node _lastNode = null!;
    private FormattedLogValuesBase _formattedLogValues = null!;
    private int _valueIndex = 0;

    private CachingFormattedLogValuesBuilder()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FormattedLogValuesBuilder AcquireAndInitialize(int formattedCount)
    {
        var instance = _cachedInstance ?? new CachingFormattedLogValuesBuilder();
        _cachedInstance = null;
        instance.Initialize(formattedCount);
        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize(int formattedCount)
    {
        _lastNode = NodeCache.Instance;
        _formattedLogValues = FormattedLogValuesBase.Create(formattedCount);
    }

    public override FormattedLogValuesBase BuildAndReset()
    {
        var templateNode = _lastNode.GetTemplateNode();
        var result = _formattedLogValues;
        result.Values[_valueIndex] = new(OriginalFormatName, templateNode.MessageTemplate);
        result.Count = _valueIndex + 1;
        result.SetSegments(templateNode.Segments);
        
        _lastNode = null!;
        _formattedLogValues = null!;
        _valueIndex = 0;

        _cachedInstance ??= this;

        return result;
    }

    public override void AppendLiteral(string str)
    {
        _lastNode = _lastNode.GetOrAddLiteralNode(str);
    }

    public override void AppendLiteralValue(in LiteralValue literalValue)
    {
        var str = literalValue.Value!;
        _lastNode = literalValue.IsCacheable 
            ? _lastNode.GetOrAddLiteralNode(str) 
            : _lastNode.CreateNotCachedLiteralNode(str);
    }

    public override void AppendFormatted(string name, object? value)
    {
        _lastNode = _lastNode.GetOrAddHoleNode(name);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }

    public override void AppendFormatted(string name, object? value, int alignment, string? format)
    {
        _lastNode = _lastNode.GetOrAddFormattedHoleNode(name, alignment, format);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }
}