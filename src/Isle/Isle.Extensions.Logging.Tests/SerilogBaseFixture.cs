using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Isle.Configuration;
using Isle.Extensions.Logging.Tests.Serilog;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Isle.Extensions.Logging.Tests;

public abstract class SerilogBaseFixture
{
    protected static readonly IReadOnlyList<LogLevel> LogLevels = EnumHelper.GetValues<LogLevel>().Where(x => x != LogLevel.None).ToArray();
        
    protected static readonly IReadOnlyList<object> FixtureArgs = (
        from logLevel in LogLevels
        from enableCaching in new[] { false, true }
        select new object[] { logLevel, enableCaching }).ToArray();

    protected static readonly IReadOnlyList<string?> Literals = new[] { null, "A", "ABC", "ABCDE", "ABCDEFGHIJKLMNOPQRSTUVWXYZ" };

    protected static readonly IReadOnlyList<object?> Scalars = new object[] { 1000, 3.5, 2.5m, "ABC" };

    protected static readonly MessageTemplateParser Parser = new ();

    private readonly List<LogEvent> _serilogLogEvents = new();

    protected SerilogBaseFixture(LogLevel minLogLevel, bool enableCaching)
    {
        MinLogLevel = minLogLevel;
        EnableCaching = enableCaching;
    }

    [OneTimeSetUp]
    protected virtual void OneTimeSetUp()
    {
        IsleConfiguration.Configure(builder => builder
            .IsResettable()
            .WithAutomaticDestructuring()
            .AddExtensionsLogging(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
        LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(MinLogLevel)
                .AddSerilog(new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .MinimumLevel.Is(ToSerilogLevel(MinLogLevel))
                    .WriteTo.TestSink(config => config.LogEvents = _serilogLogEvents)
                    .CreateLogger(), dispose: true);
        });
    }

    [OneTimeTearDown]
    protected virtual void OneTimeTearDown()
    {
        LoggerFactory.Dispose();
        IsleConfiguration.Reset();
    }

    [SetUp]
    protected virtual void Setup()
    {
        Logger = LoggerFactory.CreateLogger(GetType().FullName!);
    }

    [TearDown]
    protected virtual void TearDown()
    {
        _serilogLogEvents.Clear();
    }

    protected LogLevel MinLogLevel { get; }

    protected bool EnableCaching { get; }

    protected ILoggerFactory LoggerFactory { get; private set; } = null!;

    protected ILogger Logger { get; private set; } = null!;

    protected IReadOnlyList<LogEvent> LogEvents => _serilogLogEvents;

    protected static LogEventLevel ToSerilogLevel(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

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

        return string.Format(CultureInfo.InvariantCulture, formatString, value);
    }

    protected static EquivalencyAssertionOptions<LogEvent> LogEventEquivalency(EquivalencyAssertionOptions<LogEvent> cfg)
    {
        return cfg.Excluding(e => e.Timestamp)
            .ComparingByMembers<TextToken>()
            .ComparingByMembers<PropertyToken>()
            .Using<MessageTemplateToken>(ctx => ctx.Subject
                .Should().BeEquivalentTo(
                    ctx.Expectation,
                    config => config.RespectingRuntimeTypes()))
                .WhenTypeIs<MessageTemplateToken>()
            .Using<LogEventPropertyValue>(ctx => ctx.Subject
                .Should().BeEquivalentTo(
                    ctx.Expectation,
                    config => config.RespectingRuntimeTypes()))
                .WhenTypeIs<LogEventPropertyValue>()
            .WithTracing();
    }

    protected static EquivalencyAssertionOptions<T> MessageTemplateTokenEquivalency<T>(EquivalencyAssertionOptions<T> cfg)
        where T : MessageTemplateToken
    {
        return cfg.ComparingByMembers<TextToken>().ComparingByMembers<PropertyToken>().RespectingRuntimeTypes();
    }

    protected static EquivalencyAssertionOptions<KeyValuePair<string, LogEventPropertyValue>> PropertiesEquivalency(
        EquivalencyAssertionOptions<KeyValuePair<string, LogEventPropertyValue>> cfg)
    {
        return cfg.Using<LogEventPropertyValue>(ctx => ctx.Subject
            .Should().BeEquivalentTo(
                ctx.Expectation,
                config => config.RespectingRuntimeTypes()))
            .WhenTypeIs<LogEventPropertyValue>();
    }

    protected LogEventProperty SourceContextProperty => new LogEventProperty("SourceContext", new ScalarValue(GetType().FullName));

    protected LogEventProperty EventIdProperty(EventId eventId) =>
        new LogEventProperty("EventId",
            new StructureValue(new[] { new LogEventProperty(nameof(eventId.Id), new ScalarValue(eventId.Id)) }));
}