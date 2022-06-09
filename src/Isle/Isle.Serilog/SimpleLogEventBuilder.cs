using System.Globalization;
using System.Runtime.CompilerServices;
using Isle.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Serilog;

internal sealed class SimpleLogEventBuilder : LogEventBuilder
{
    private const char DestructureOperator = '@';
    private const char StringifyOperator = '$';

    private List<MessageTemplateToken> _tokens = null!;
    private LogEventProperty[] _properties = null!;
    private ILogger _logger = null!;
    private int _currentPosition;
    private int _propertyIndex;

    public override bool IsCaching => false;

    protected override void Initialize(int literalLength, int formattedCount, ILogger logger)
    {
        _tokens = new List<MessageTemplateToken>(formattedCount * 2 + 1);
        _properties = new LogEventProperty[formattedCount];
        _logger = logger;
    }

    protected override LogEvent BuildAndReset(LogEventLevel level, Exception? exception = null)
    {
        var logEvent = new LogEvent(DateTimeOffset.Now, level, exception, new MessageTemplate(_tokens), _properties);
        _tokens = null!;
        _properties = null!;
        _currentPosition = 0;
        _propertyIndex = 0;
        return logEvent;
    }

    public override void AppendLiteral(string? literal)
    {
        if (string.IsNullOrEmpty(literal))
            return;

        string escapedLiteral = LiteralUtils.EscapeLiteral(literal);
        _tokens.Add(new TextToken(literal, _currentPosition));
#warning TODO: Check whether to add literal.Length or escapedLiteral.Length
        _currentPosition += escapedLiteral.Length;
    }

    public override void AppendFormatted<T>(string name, T value, int alignment, string? format)
    {
        string originalName = name = IsleConfiguration.Current.ValueNameConverter(name);
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
            destructuring = GetDestructuring(
                IsleConfiguration.Current.ValueRepresentationPolicy.GetRepresentationOfType<T>());
        }

        if (name == string.Empty)
            name = _propertyIndex.ToString(CultureInfo.InvariantCulture);

        var rawText = GetRawText(originalName, alignment, format);
        AppendProperty(name, value, alignment, format, rawText, destructuring);
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

        // Pass namedLogValue.Name as it may contain destructure or stringify operator.
        var rawText = GetRawText(namedLogValue.Name, alignment, format);
        AppendProperty(name, namedLogValue.Value, alignment, format, rawText, destructuring);
    }

    [SkipLocalsInit]
    private static string GetRawText(string name, int alignment, string? format)
    {
        int bufferLength = name.Length + (format?.Length ?? 0) + 15;
        var vsb = bufferLength <= 256
            ? new ValueStringBuilder(stackalloc char[bufferLength])
            : new ValueStringBuilder(bufferLength);

        vsb.Append('{');
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

        if (_logger.BindProperty(name, value, destructuring == Destructuring.Destructure, out var property))
        {
            _properties[_propertyIndex++] = property;
        }
        else
        {
            // We should actually never get here.
            _properties[_propertyIndex++] = new LogEventProperty(name, new ScalarValue(property));
        }
    }

}