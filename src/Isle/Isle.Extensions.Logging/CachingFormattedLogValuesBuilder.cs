using System.Runtime.CompilerServices;
using Isle.Extensions.Logging.Caching;

namespace Isle.Extensions.Logging;

internal sealed class CachingFormattedLogValuesBuilder : IFormattedLogValuesBuilder
{
    [ThreadStatic]
    private static CachingFormattedLogValuesBuilder? _cachedInstance;

    private Node _lastNode = null!;
    private FormattedLogValuesBase _formattedLogValues = null!;
    private int _valueIndex = 0;

    private CachingFormattedLogValuesBuilder()
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static CachingFormattedLogValuesBuilder AcquireAndInitialize(FormattedLogValuesBase formattedLogValues)
    {
        var instance = _cachedInstance ?? new CachingFormattedLogValuesBuilder();
        _cachedInstance = null;
        instance.Initialize(formattedLogValues);
        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize(FormattedLogValuesBase formattedLogValues)
    {
        _formattedLogValues = formattedLogValues;
        _lastNode = NodeCache.Instance;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public FormattedLogValuesBase BuildAndReset()
    {
        var templateNode = _lastNode.GetTemplateNode();
        var result = _formattedLogValues;
        result.Values[_valueIndex] = new(FormattedLogValuesBuilder.OriginalFormatName, templateNode.MessageTemplate);
        result.Count = _valueIndex + 1;
        result.SetSegments(templateNode.Segments);
        
        _lastNode = null!;
        _formattedLogValues = null!;
        _valueIndex = 0;

        _cachedInstance ??= this;

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLiteral(string str)
    {
        _lastNode = _lastNode.GetOrAddLiteralNode(str);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AppendLiteralValue(in LiteralValue literalValue)
    {
        var str = literalValue.Value!;
        _lastNode = literalValue.IsCacheable 
            ? _lastNode.GetOrAddLiteralNode(str) 
            : _lastNode.CreateNotCachedLiteralNode(str);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AppendFormatted(string name, object? value)
    {
        _lastNode = _lastNode.GetOrAddHoleNode(name);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void AppendFormatted(string name, object? value, int alignment, string? format)
    {
        _lastNode = _lastNode.GetOrAddFormattedHoleNode(name, alignment, format);
        _formattedLogValues.Values[_valueIndex++] = new(name, value);
    }
}