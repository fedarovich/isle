using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Isle.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Serilog;

internal sealed class SimpleLogEventBuilder : LogEventBuilder
{
    [ThreadStatic]
    private static SimpleLogEventBuilder? _cachedInstance;

    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';
    private const int MaxStringBuilderCapacity = 512;

    private StringBuilder? _cachedStringBuilder;

    private List<MessageTemplateToken> _tokens = null!;
    private LogEventProperty[] _properties = null!;
    private ILogger _logger = null!;
    private StringBuilder _messageTemplateBuilder = null!;
    private int _currentPosition;
    private int _propertyIndex;

    private SimpleLogEventBuilder()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LogEventBuilder AcquireAndInitialize(int literalLength, int formattedCount, ILogger logger)
    {
        var instance = _cachedInstance ?? new SimpleLogEventBuilder();
        _cachedInstance = null;
        instance.Initialize(literalLength, formattedCount, logger);
        return instance;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize(int literalLength, int formattedCount, ILogger logger)
    {
        _tokens = new List<MessageTemplateToken>(formattedCount * 2 + 1);
        _properties = new LogEventProperty[formattedCount];
        _messageTemplateBuilder = AcquireStringBuilder();
        _logger = logger;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        StringBuilder AcquireStringBuilder()
        {
            var capacity = Math.Max(literalLength + formattedCount * 16, MaxStringBuilderCapacity);
            var cachedStringBuilder = _cachedStringBuilder;
            if (cachedStringBuilder == null || cachedStringBuilder.Capacity < capacity)
                return new StringBuilder(capacity);

            _cachedStringBuilder = null;
            return cachedStringBuilder;
        }
    }

    public override LogEvent BuildAndReset(LogEventLevel level, Exception? exception = null)
    {
        var logEvent = new LogEvent(
            DateTimeOffset.Now, 
            level, 
            exception, 
            new MessageTemplate(GetStringAndRelease(_messageTemplateBuilder), _tokens),
            _properties.Length == _propertyIndex ? _properties : _properties.Take(_propertyIndex));
        _tokens = null!;
        _properties = null!;
        _currentPosition = 0;
        _propertyIndex = 0;
        
        _cachedInstance ??= this;

        return logEvent;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        string GetStringAndRelease(StringBuilder stringBuilder)
        {
            var str = stringBuilder.ToString();
            if (stringBuilder.Capacity <= MaxStringBuilderCapacity)
            {
                stringBuilder.Clear();
                _cachedStringBuilder = stringBuilder;
            }

            return str;
        }
    }

    public override void AppendLiteral(string literal)
    {
        _tokens.Add(new TextToken(literal, _currentPosition));
        var start = _messageTemplateBuilder.Length;
        _messageTemplateBuilder.EscapeAndAppend(literal.AsSpan());
        var end = _messageTemplateBuilder.Length;
        _currentPosition += end - start;
    }

    public override void AppendLiteralValue(in LiteralValue literalValue)
    {
        AppendLiteral(literalValue.Value!);
    }

    public override void AppendFormatted<T>(string name, T value)
    {
        name = CoreConfiguration.ConvertValueName(name);
        Destructuring destructuring;
        if (name.StartsWith(DestructureOperator))
        {
            destructuring = Destructuring.Destructure;
            name = name.Substring(1);
        }
        else if (name.StartsWith(StringifyOperator))
        {
            destructuring = Destructuring.Stringify;
            name = name.Substring(1);
        }
        else
        {
            destructuring = GetDestructuring(CoreConfiguration.GetRepresentationOfType<T>());
        }

        if (name == string.Empty)
            name = _propertyIndex.ToString(CultureInfo.InvariantCulture);

        var rawText = GetRawText(name, destructuring);
        _messageTemplateBuilder.Append(rawText);
        AppendProperty(name, value, 0, null, rawText, destructuring);
    }

    public override void AppendFormatted<T>(string name, T value, int alignment, string? format)
    {
        name = CoreConfiguration.ConvertValueName(name);
        Destructuring destructuring;
        if (name.StartsWith(DestructureOperator))
        {
            destructuring = Destructuring.Destructure;
            name = name.Substring(1);
        }
        else if (name.StartsWith(StringifyOperator))
        {
            destructuring = Destructuring.Stringify;
            name = name.Substring(1);
        }
        else
        {
            destructuring = GetDestructuring(CoreConfiguration.GetRepresentationOfType<T>());
        }

        if (name == string.Empty)
            name = _propertyIndex.ToString(CultureInfo.InvariantCulture);

        var rawText = GetRawText(name, alignment, format, destructuring);
        _messageTemplateBuilder.Append(rawText);
        AppendProperty(name, value, alignment, format, rawText, destructuring);
    }

    public override void AppendFormatted(in NamedLogValue namedLogValue)
    {
        var destructuring = Destructuring.Default;
        var name = namedLogValue.Name;
        if (namedLogValue.Name.StartsWith(DestructureOperator))
        {
            destructuring = Destructuring.Destructure;
            name = ReferenceEquals(namedLogValue.Name, namedLogValue.RawName)
                ? namedLogValue.Name.Substring(1)
                : namedLogValue.RawName;
        }
        else if (namedLogValue.Name.StartsWith(StringifyOperator))
        {
            destructuring = Destructuring.Stringify;
            name = ReferenceEquals(namedLogValue.Name, namedLogValue.RawName)
                ? namedLogValue.Name.Substring(1)
                : namedLogValue.RawName;
        }

        var rawText = GetRawText(name, destructuring);
        _messageTemplateBuilder.Append(rawText);
        AppendProperty(name, namedLogValue.Value, 0, null, rawText, destructuring);
    }

    public override void AppendFormatted(in NamedLogValue namedLogValue, int alignment, string? format)
    {
        var destructuring = Destructuring.Default;
        var name = namedLogValue.Name;
        if (namedLogValue.Name.StartsWith(DestructureOperator))
        {
            destructuring = Destructuring.Destructure;
            name = ReferenceEquals(namedLogValue.Name, namedLogValue.RawName)
                ? namedLogValue.Name.Substring(1)
                : namedLogValue.RawName;
        }
        else if (namedLogValue.Name.StartsWith(StringifyOperator))
        {
            destructuring = Destructuring.Stringify;
            name = ReferenceEquals(namedLogValue.Name, namedLogValue.RawName)
                ? namedLogValue.Name.Substring(1)
                : namedLogValue.RawName;
        }

        var rawText = GetRawText(name, alignment, format, destructuring);
        _messageTemplateBuilder.Append(rawText);
        AppendProperty(name, namedLogValue.Value, alignment, format, rawText, destructuring);
    }

    [SkipLocalsInit]
    private static string GetRawText(string name, Destructuring destructuring)
    {
        int bufferLength = name.Length + 3;
        var vsb = bufferLength <= 256
            ? new ValueStringBuilder(stackalloc char[bufferLength])
            : new ValueStringBuilder(bufferLength);

        vsb.Append('{');
        switch (destructuring)
        {
            case Destructuring.Destructure:
                vsb.Append(DestructureOperator);
                break;
            case Destructuring.Stringify:
                vsb.Append(StringifyOperator);
                break;
        }
        vsb.Append(name);

        vsb.Append('}');
        var rawText = vsb.ToString();
        return rawText;
    }

    [SkipLocalsInit]
    private static string GetRawText(string name, int alignment, string? format, Destructuring destructuring)
    {
        int bufferLength = name.Length + (format?.Length ?? 0) + 16;
        var vsb = bufferLength <= 256
            ? new ValueStringBuilder(stackalloc char[bufferLength])
            : new ValueStringBuilder(bufferLength);

        vsb.Append('{');
        switch (destructuring)
        {
            case Destructuring.Destructure:
                vsb.Append(DestructureOperator);
                break;
            case Destructuring.Stringify:
                vsb.Append(StringifyOperator);
                break;
        }
        vsb.Append(name);

        if (alignment != 0)
        {
            vsb.Append(',');
            vsb.AppendSpanFormattable(alignment);
        }

        if (!string.IsNullOrEmpty(format))
        {
            vsb.Append(':');
            vsb.Append(format);
        }

        vsb.Append('}');
        var rawText = vsb.ToString();
        return rawText;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Destructuring GetDestructuring(ValueRepresentation representation) =>
        representation switch
        {
            ValueRepresentation.Default => Destructuring.Default,
            ValueRepresentation.Destructure => Destructuring.Destructure,
            ValueRepresentation.Stringify => Destructuring.Stringify,
            _ => throw new ArgumentOutOfRangeException(nameof(representation))
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Alignment GetAlignment(int alignment) => alignment <= 0
        ? new Alignment(AlignmentDirection.Left, -alignment)
        : new Alignment(AlignmentDirection.Right, alignment);

    private void AppendProperty(string name, object? value, int alignment, string? format, string rawText, Destructuring destructuring)
    {
        var propertyToken = new PropertyToken(
            name,
            rawText,
            format,
            GetAlignment(alignment),
            destructuring,
            _currentPosition);
        _tokens.Add(propertyToken);

        _currentPosition += rawText.Length;

        if (destructuring != Destructuring.Stringify)
        {
            if (_logger.BindProperty(name, value, destructuring == Destructuring.Destructure, out var property))
            {
                _properties[_propertyIndex++] = property;
                return;
            }
        }
        // Unfortunately, BindProperty does not handle Stringify option correctly, so we have a separate, less efficient branch for it.
        else
        {
            var pooledValueArray = AcquirePooledValueArray(value);
            try
            {
                if (_logger.BindMessageTemplate(rawText, pooledValueArray, out _, out var properties))
                {
                    _properties[_propertyIndex++] = properties.First();
                    return;
                }
            }
            finally
            {
                ReleasePooledValueArray();
            }
        }

        // We should actually never get here, but if we suddenly do, we'll just log the string representation of the value. 
        _properties[_propertyIndex++] = new LogEventProperty(name, new ScalarValue(value?.ToString()));
    }
}