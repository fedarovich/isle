using System.Linq;
using System.Runtime.CompilerServices;
using Serilog.Events;

namespace Isle.Extensions.Logging.Tests;

internal static class SerilogTestExtensions
{
    public static LogEventProperty ToLogEventProperty<T>(
        this T value,
        [CallerArgumentExpression("value")] string name = "")
    {
        return new LogEventProperty(
            name, 
            value != null ? ToLogEventPropertyValue((dynamic) value) : new ScalarValue(null));
    }

    private static LogEventPropertyValue ToLogEventPropertyValue(TestObject value) =>
        new StructureValue(
            new[]
            {
                new LogEventProperty(nameof(value.X), ToLogEventPropertyValue((dynamic) value.X)),
                new LogEventProperty(nameof(value.Y), ToLogEventPropertyValue((dynamic) value.Y))
            },
            nameof(TestObject));

    private static LogEventPropertyValue ToLogEventPropertyValue<T>(T value) where T : unmanaged => new ScalarValue(value);

    private static LogEventPropertyValue ToLogEventPropertyValue(string value) => new ScalarValue(value);

    private static LogEventPropertyValue ToLogEventPropertyValue<T>(T[] values) =>
        new SequenceValue(values.Select(v => v != null 
            ? (LogEventPropertyValue) ToLogEventPropertyValue((dynamic) v)
            : new ScalarValue(null)));
}