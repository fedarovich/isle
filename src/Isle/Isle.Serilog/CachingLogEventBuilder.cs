using System.Buffers;
using System.Diagnostics;
using Isle.Configuration;
using Isle.Extensions;
using Isle.Serilog.Caching;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Serilog;

internal sealed class CachingLogEventBuilder : LogEventBuilder
{
    private readonly object?[] _pooledValueArray = new object?[1];

    private ILogger _logger = null!;
    private Node _lastNode = null!;
    private object?[] _propertyValues = null!;
    private int _propertyIndex;

    public override bool IsCaching => true;

    protected override void Initialize(int literalLength, int formattedCount, ILogger logger)
    {
        _logger = logger;
        _lastNode = NodeCache.Instance;
        _propertyValues = formattedCount > 0 
            ? ArrayPool<object?>.Shared.Rent(formattedCount)
            : Array.Empty<object?>();
        _propertyIndex = 0;
    }

    protected override LogEvent BuildAndReset(LogEventLevel level, Exception? exception = null)
    {
        var templateNode = _lastNode.GetTemplateNode();
        
        Debug.Assert(templateNode.PropertyNodes.Count == _propertyIndex);
        var properties = new LogEventProperty[_propertyIndex];
        for (int i = 0; i < properties.Length && i < _propertyValues.Length; i++)
        {
            properties[i] = BindLogEventProperty(templateNode.PropertyNodes[i], _propertyValues[i]);
        }

        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            level,
            exception,
            templateNode.MessageTemplate,
            properties);

        _logger = null!;
        _lastNode = null!;
        _propertyValues = null!;

        return logEvent;
    }

    public override void AppendLiteral(string? str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            _lastNode = _lastNode.GetOrAddTextNode(str);
        }
    }

    public override void AppendFormatted<T>(string name, T value, int alignment, string? format)
    {
        AppendFormatted(value.Named(IsleConfiguration.Current.ValueNameConverter(name), false), alignment, format);
    }

    public override void AppendFormatted(in NamedLogValue namedLogValue, int alignment, string? format)
    {
        _lastNode = (alignment == 0 && string.IsNullOrEmpty(format))
            ? _lastNode.GetOrAddPropertyNode(namedLogValue.Name, namedLogValue.RawName)
            : _lastNode.GetOrAddPropertyNode(namedLogValue.Name, namedLogValue.RawName, alignment, format);
        _propertyValues[_propertyIndex++] = namedLogValue.Value;
    }

    private LogEventProperty BindLogEventProperty(PropertyNode node, object? value)
    {
        if (node.Token.Destructuring != Destructuring.Stringify)
        {
            if (_logger.BindProperty(node.Token.PropertyName, value, node.Token.Destructuring == Destructuring.Destructure, out var property))
                return property;
        }
        // Unfortunately, BindProperty does not handle Stringify option correctly, so we have a separate, less efficient branch for it.
        else
        {
            _pooledValueArray[0] = value;
            try
            {
                if (_logger.BindMessageTemplate(node.RawText, _pooledValueArray, out _, out var properties))
                {
                    return properties.First();
                }
            }
            finally
            {
                _pooledValueArray[0] = null;
            }
        }

        // We should actually never get here, but if we suddenly do, we'll just log the string representation of the value. 
       return new LogEventProperty(node.Token.PropertyName, new ScalarValue(value?.ToString()));
    }
}