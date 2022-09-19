using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Isle.Configuration;
using Isle.Extensions;
using Isle.Serilog.Caching;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Serilog;

internal sealed class CachingLogEventBuilder : LogEventBuilder
{
    [ThreadStatic]
    private static CachingLogEventBuilder? _cachedInstance;

    private ILogger _logger = null!;
    private Node _lastNode = null!;
    private object?[] _propertyValues = null!;
    private IsleConfiguration _configuration = null!;
    private int _propertyIndex;

    private CachingLogEventBuilder()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LogEventBuilder AcquireAndInitialize(int formattedCount, ILogger logger)
    {
        var instance = _cachedInstance ?? new CachingLogEventBuilder();
        _cachedInstance = null;
        instance.Initialize(formattedCount, logger);
        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize(int formattedCount, ILogger logger)
    {
        _logger = logger;
        _lastNode = NodeCache.Instance;
        _propertyValues = formattedCount > 0 
            ? ArrayPool<object?>.Shared.Rent(formattedCount)
            : Array.Empty<object?>();
        _propertyIndex = 0;
        _configuration = IsleConfiguration.Current;
    }

    public override LogEvent BuildAndReset(LogEventLevel level, Exception? exception = null)
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
        _configuration = null!;

        _cachedInstance ??= this;

        return logEvent;
    }

    public override void AppendLiteral(string str)
    {
        _lastNode = _lastNode.GetOrAddTextNode(str);
    }

    public override void AppendLiteralValue(in LiteralValue literalValue)
    {
        var str = literalValue.Value!;
        _lastNode = literalValue.IsCacheable 
            ? _lastNode.GetOrAddTextNode(str) 
            : _lastNode.CreateNotCachedTextNode(str);
    }

    public override void AppendFormatted<T>(string name, T value)
    {
        AppendFormatted(value.Named(_configuration.ConvertValueName(name), false, _configuration));
    }

    public override void AppendFormatted<T>(string name, T value, int alignment, string? format)
    {
        AppendFormatted(value.Named(_configuration.ConvertValueName(name), false, _configuration), alignment, format);
    }

    public override void AppendFormatted(in NamedLogValue namedLogValue)
    {
        _lastNode = _lastNode.GetOrAddPropertyNode(namedLogValue.Name, namedLogValue.RawName);
        _propertyValues[_propertyIndex++] = namedLogValue.Value;
    }

    public override void AppendFormatted(in NamedLogValue namedLogValue, int alignment, string? format)
    {
        _lastNode = _lastNode.GetOrAddPropertyNode(namedLogValue.Name, namedLogValue.RawName, alignment, format);
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
            var pooledValueArray = AcquirePooledValueArray(value);
            try
            {
                if (_logger.BindMessageTemplate(node.RawText, pooledValueArray, out _, out var properties))
                {
                    return properties.First();
                }
            }
            finally
            {
                ReleasePooledValueArray();
            }
        }

        // We should actually never get here, but if we suddenly do, we'll just log the string representation of the value. 
       return new LogEventProperty(node.Token.PropertyName, new ScalarValue(value?.ToString()));
    }
}