using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Serilog.Events;

namespace Isle.Serilog.Tests;

internal static class TestExtensions
{
    public static KeyValuePair<string, LogEventPropertyValue> ToLogEventPropertyValue<T>(
        this T value,
        [CallerArgumentExpression("value")] string name = "")
    {
        return new KeyValuePair<string, LogEventPropertyValue>(
            name, 
            value != null ? ToLogEventPropertyValueCore((dynamic) value) : new ScalarValue(null));
    }

    private static LogEventPropertyValue ToLogEventPropertyValueCore(TestObject value) =>
        new StructureValue(
            new[]
            {
                new LogEventProperty(nameof(value.X), ToLogEventPropertyValueCore((dynamic) value.X)),
                new LogEventProperty(nameof(value.Y), ToLogEventPropertyValueCore((dynamic) value.Y))
            },
            nameof(TestObject));

    private static LogEventPropertyValue ToLogEventPropertyValueCore<T>(T value) where T : unmanaged => new ScalarValue(value);

    private static LogEventPropertyValue ToLogEventPropertyValueCore(string value) => new ScalarValue(value);

    private static LogEventPropertyValue ToLogEventPropertyValueCore<T>(T[] values) =>
        new SequenceValue(values.Select(v => v != null 
            ? (LogEventPropertyValue) ToLogEventPropertyValueCore((dynamic) v)
            : new ScalarValue(null)));
}