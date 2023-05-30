namespace Isle.Extensions.Logging;

internal interface IFormattedLogValuesBuilder
{
    FormattedLogValuesBase BuildAndReset();
    void AppendLiteral(string str);
    void AppendLiteralValue(in LiteralValue literalValue);
    void AppendFormatted(string name, object? value);
    void AppendFormatted(string name, object? value, int alignment, string? format);
}