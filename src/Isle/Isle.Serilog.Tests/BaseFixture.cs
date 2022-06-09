using System;
using System.Collections.Generic;
using System.Linq;
using Isle.Configuration;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Serilog.Tests;

public abstract class BaseFixture
{
    protected static readonly IReadOnlyList<LogEventLevel> LogEventLevels = Enum.GetValues<LogEventLevel>().ToArray();
        
    protected static readonly IReadOnlyList<object> FixtureArgs = (
        from logEventLevel in LogEventLevels
        from enableCaching in new[] { false, true }
        select new object[] { logEventLevel, enableCaching }).ToArray();

    protected static readonly IReadOnlyList<string?> Literals = new[] { null, "A", "ABC", "ABCDE", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };

    protected static readonly IReadOnlyList<object?> Scalars = new object[] { 1000, 3.5, 2.5m, "ABC" };

    protected static readonly MessageTemplateParser Parser = new ();

    private readonly List<LogEvent> _logEvents = new();

    protected BaseFixture(LogEventLevel minLogEventLevel, bool enableCaching)
    {
        MinLogEventLevel = minLogEventLevel;
        EnableCaching = enableCaching;
    }

    [OneTimeSetUp]
    protected virtual void OneTimeSetUp()
    {
        IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring()
            //.ConfigureSerilog(cfg => cfg.EnableMessageTemplateCaching = EnableCaching)
        );
        Logger = new LoggerConfiguration()
            .WriteTo.TestSink(opt => opt.LogEvents = _logEvents)
            .MinimumLevel.Is(MinLogEventLevel)
            .CreateLogger();
    }

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        (Logger as IDisposable)?.Dispose();
        IsleConfiguration.Reset();
    }

    [SetUp]
    protected virtual void Setup()
    {
    }

    [TearDown]
    protected virtual void TearDown()
    {
        _logEvents.Clear();
    }

    protected LogEventLevel MinLogEventLevel { get; }

    protected bool EnableCaching { get; }

    protected ILogger Logger { get; private set; } = null!;

    protected IReadOnlyList<LogEvent> LogEvents => _logEvents;

    protected string Format(dynamic? value, int alignment, string format = "") => FormatCore(value, alignment, format);

    protected string Format(dynamic? value, string format) => Format(value, 0, format);

    protected string Format(dynamic? value) => Format(value, 0, "");

    private string FormatCore(string str, int alignment, string format) => "\"" + str + "\"";

    private string FormatCore<T>(T[] values, int alignment, string format) =>
        "[" + string.Join(", ", values.Select(v => Format(v, 0, format))) + "]";

    private string FormatCore(TestObject t, int alignment, string format) => $"TestObject {{ X: {t.X}, Y: {t.Y} }}";

    private string FormatCore<T>(T value, int alignment, string format)
    {
        string formatString = "{0";
        if (alignment != 0)
        {
            formatString += "," + alignment;
        }

        if (!string.IsNullOrEmpty(format))
        {
            formatString += ":" + format;
        }

        formatString += "}";

        return string.Format(formatString, value);
    }

    protected record TestObject(int X, int Y);
}