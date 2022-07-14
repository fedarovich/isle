#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using FluentAssertions;
using Serilog.Events;

namespace Isle.Extensions.Logging.Tests;

[TestFixtureSource(nameof(FixtureArgs))]
public class SerilogLoggerExtensionsTests : SerilogBaseFixture
{
    public SerilogLoggerExtensionsTests(LogLevel minLogLevel, bool enableCaching) : base(minLogLevel, enableCaching)
    {
    }


    #region LogTrace


    [Test]
    public void LogTrace_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        
        
        Logger.LogTrace($"Test");
        Logger.LogTrace(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "T{{es}}t";

        
        
        Logger.LogTrace($"T{{es}}t");
        Logger.LogTrace(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogTrace_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        
        Logger.LogTrace($"{value}");
        Logger.LogTrace("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        
        Logger.LogTrace($"{value,3}");
        Logger.LogTrace("{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        
        Logger.LogTrace($"{value:N}");
        Logger.LogTrace("{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        
        Logger.LogTrace($"{value,8:N}");
        Logger.LogTrace("{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogTrace_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        
        Logger.LogTrace($"{value}");
        Logger.LogTrace("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogTrace_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogTrace($"{value}");
        Logger.LogTrace("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_String()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        
        
        Logger.LogTrace($"{value}");
        Logger.LogTrace("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_Mixed()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.LogTrace(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogTrace(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogTrace_Named()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        
        Logger.LogTrace(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogTrace(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogTrace_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"Test");
        Logger.LogTrace(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"T{{es}}t");
        Logger.LogTrace(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogTrace_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");
        Logger.LogTrace(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value,3}");
        Logger.LogTrace(exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value:N}");
        Logger.LogTrace(exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value,8:N}");
        Logger.LogTrace(exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogTrace_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");
        Logger.LogTrace(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogTrace_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");
        Logger.LogTrace(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");
        Logger.LogTrace(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_Mixed_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogTrace(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogTrace_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogTrace(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogTrace_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"Test");
        Logger.LogTrace(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"T{{es}}t");
        Logger.LogTrace(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogTrace_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");
        Logger.LogTrace(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value,3}");
        Logger.LogTrace(eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value:N}");
        Logger.LogTrace(eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value,8:N}");
        Logger.LogTrace(eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogTrace_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");
        Logger.LogTrace(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogTrace_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");
        Logger.LogTrace(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");
        Logger.LogTrace(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_Mixed_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.LogTrace(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogTrace(eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogTrace_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogTrace(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogTrace(eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogTrace_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"Test");
        Logger.LogTrace(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"T{{es}}t");
        Logger.LogTrace(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogTrace_Scalar_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value}");
        Logger.LogTrace(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value,3}");
        Logger.LogTrace(eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value:N}");
        Logger.LogTrace(eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value,8:N}");
        Logger.LogTrace(eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogTrace_Complex_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value}");
        Logger.LogTrace(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogTrace_Collection_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value}");
        Logger.LogTrace(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_String_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{value}");
        Logger.LogTrace(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogTrace_Mixed_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogTrace(eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogTrace_Named_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogTrace(eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // LogTrace


    #region LogDebug


    [Test]
    public void LogDebug_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        
        
        Logger.LogDebug($"Test");
        Logger.LogDebug(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "T{{es}}t";

        
        
        Logger.LogDebug($"T{{es}}t");
        Logger.LogDebug(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogDebug_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        
        Logger.LogDebug($"{value}");
        Logger.LogDebug("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        
        Logger.LogDebug($"{value,3}");
        Logger.LogDebug("{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        
        Logger.LogDebug($"{value:N}");
        Logger.LogDebug("{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        
        Logger.LogDebug($"{value,8:N}");
        Logger.LogDebug("{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogDebug_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        
        Logger.LogDebug($"{value}");
        Logger.LogDebug("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogDebug_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogDebug($"{value}");
        Logger.LogDebug("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_String()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        
        
        Logger.LogDebug($"{value}");
        Logger.LogDebug("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_Mixed()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.LogDebug(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogDebug(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogDebug_Named()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        
        Logger.LogDebug(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogDebug(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogDebug_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"Test");
        Logger.LogDebug(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"T{{es}}t");
        Logger.LogDebug(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogDebug_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");
        Logger.LogDebug(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value,3}");
        Logger.LogDebug(exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value:N}");
        Logger.LogDebug(exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value,8:N}");
        Logger.LogDebug(exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogDebug_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");
        Logger.LogDebug(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogDebug_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");
        Logger.LogDebug(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");
        Logger.LogDebug(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_Mixed_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogDebug(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogDebug_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogDebug(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogDebug_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"Test");
        Logger.LogDebug(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"T{{es}}t");
        Logger.LogDebug(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogDebug_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");
        Logger.LogDebug(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value,3}");
        Logger.LogDebug(eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value:N}");
        Logger.LogDebug(eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value,8:N}");
        Logger.LogDebug(eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogDebug_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");
        Logger.LogDebug(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogDebug_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");
        Logger.LogDebug(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");
        Logger.LogDebug(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_Mixed_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.LogDebug(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogDebug(eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogDebug_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogDebug(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogDebug(eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogDebug_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"Test");
        Logger.LogDebug(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"T{{es}}t");
        Logger.LogDebug(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogDebug_Scalar_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value}");
        Logger.LogDebug(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value,3}");
        Logger.LogDebug(eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value:N}");
        Logger.LogDebug(eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value,8:N}");
        Logger.LogDebug(eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogDebug_Complex_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value}");
        Logger.LogDebug(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogDebug_Collection_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value}");
        Logger.LogDebug(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_String_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{value}");
        Logger.LogDebug(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogDebug_Mixed_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogDebug(eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogDebug_Named_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogDebug(eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // LogDebug


    #region LogInformation


    [Test]
    public void LogInformation_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        
        
        Logger.LogInformation($"Test");
        Logger.LogInformation(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "T{{es}}t";

        
        
        Logger.LogInformation($"T{{es}}t");
        Logger.LogInformation(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogInformation_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        
        Logger.LogInformation($"{value}");
        Logger.LogInformation("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        
        Logger.LogInformation($"{value,3}");
        Logger.LogInformation("{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        
        Logger.LogInformation($"{value:N}");
        Logger.LogInformation("{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        
        Logger.LogInformation($"{value,8:N}");
        Logger.LogInformation("{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogInformation_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        
        Logger.LogInformation($"{value}");
        Logger.LogInformation("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogInformation_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogInformation($"{value}");
        Logger.LogInformation("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_String()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        
        
        Logger.LogInformation($"{value}");
        Logger.LogInformation("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_Mixed()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.LogInformation(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogInformation(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogInformation_Named()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        
        Logger.LogInformation(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogInformation(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogInformation_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"Test");
        Logger.LogInformation(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"T{{es}}t");
        Logger.LogInformation(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogInformation_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");
        Logger.LogInformation(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value,3}");
        Logger.LogInformation(exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value:N}");
        Logger.LogInformation(exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value,8:N}");
        Logger.LogInformation(exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogInformation_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");
        Logger.LogInformation(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogInformation_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");
        Logger.LogInformation(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");
        Logger.LogInformation(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_Mixed_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogInformation(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogInformation_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogInformation(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogInformation_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"Test");
        Logger.LogInformation(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"T{{es}}t");
        Logger.LogInformation(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogInformation_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");
        Logger.LogInformation(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value,3}");
        Logger.LogInformation(eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value:N}");
        Logger.LogInformation(eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value,8:N}");
        Logger.LogInformation(eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogInformation_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");
        Logger.LogInformation(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogInformation_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");
        Logger.LogInformation(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");
        Logger.LogInformation(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_Mixed_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.LogInformation(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogInformation(eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogInformation_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogInformation(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogInformation(eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogInformation_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"Test");
        Logger.LogInformation(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"T{{es}}t");
        Logger.LogInformation(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogInformation_Scalar_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value}");
        Logger.LogInformation(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value,3}");
        Logger.LogInformation(eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value:N}");
        Logger.LogInformation(eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value,8:N}");
        Logger.LogInformation(eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogInformation_Complex_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value}");
        Logger.LogInformation(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogInformation_Collection_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value}");
        Logger.LogInformation(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_String_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{value}");
        Logger.LogInformation(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogInformation_Mixed_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogInformation(eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogInformation_Named_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogInformation(eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // LogInformation


    #region LogWarning


    [Test]
    public void LogWarning_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        
        
        Logger.LogWarning($"Test");
        Logger.LogWarning(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "T{{es}}t";

        
        
        Logger.LogWarning($"T{{es}}t");
        Logger.LogWarning(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogWarning_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        
        Logger.LogWarning($"{value}");
        Logger.LogWarning("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        
        Logger.LogWarning($"{value,3}");
        Logger.LogWarning("{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        
        Logger.LogWarning($"{value:N}");
        Logger.LogWarning("{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        
        Logger.LogWarning($"{value,8:N}");
        Logger.LogWarning("{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogWarning_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        
        Logger.LogWarning($"{value}");
        Logger.LogWarning("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogWarning_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogWarning($"{value}");
        Logger.LogWarning("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_String()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        
        
        Logger.LogWarning($"{value}");
        Logger.LogWarning("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_Mixed()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.LogWarning(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogWarning(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogWarning_Named()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        
        Logger.LogWarning(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogWarning(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogWarning_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"Test");
        Logger.LogWarning(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"T{{es}}t");
        Logger.LogWarning(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogWarning_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");
        Logger.LogWarning(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value,3}");
        Logger.LogWarning(exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value:N}");
        Logger.LogWarning(exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value,8:N}");
        Logger.LogWarning(exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogWarning_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");
        Logger.LogWarning(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogWarning_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");
        Logger.LogWarning(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");
        Logger.LogWarning(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_Mixed_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogWarning(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogWarning_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogWarning(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogWarning_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"Test");
        Logger.LogWarning(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"T{{es}}t");
        Logger.LogWarning(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogWarning_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");
        Logger.LogWarning(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value,3}");
        Logger.LogWarning(eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value:N}");
        Logger.LogWarning(eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value,8:N}");
        Logger.LogWarning(eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogWarning_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");
        Logger.LogWarning(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogWarning_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");
        Logger.LogWarning(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");
        Logger.LogWarning(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_Mixed_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.LogWarning(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogWarning(eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogWarning_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogWarning(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogWarning(eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogWarning_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"Test");
        Logger.LogWarning(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"T{{es}}t");
        Logger.LogWarning(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogWarning_Scalar_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value}");
        Logger.LogWarning(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value,3}");
        Logger.LogWarning(eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value:N}");
        Logger.LogWarning(eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value,8:N}");
        Logger.LogWarning(eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogWarning_Complex_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value}");
        Logger.LogWarning(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogWarning_Collection_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value}");
        Logger.LogWarning(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_String_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{value}");
        Logger.LogWarning(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogWarning_Mixed_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogWarning(eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogWarning_Named_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogWarning(eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // LogWarning


    #region LogError


    [Test]
    public void LogError_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        
        
        Logger.LogError($"Test");
        Logger.LogError(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "T{{es}}t";

        
        
        Logger.LogError($"T{{es}}t");
        Logger.LogError(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogError_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        
        Logger.LogError($"{value}");
        Logger.LogError("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        
        Logger.LogError($"{value,3}");
        Logger.LogError("{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        
        Logger.LogError($"{value:N}");
        Logger.LogError("{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        
        Logger.LogError($"{value,8:N}");
        Logger.LogError("{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogError_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        
        Logger.LogError($"{value}");
        Logger.LogError("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogError_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogError($"{value}");
        Logger.LogError("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_String()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        
        
        Logger.LogError($"{value}");
        Logger.LogError("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_Mixed()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.LogError(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogError(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogError_Named()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        
        Logger.LogError(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogError(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogError_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"Test");
        Logger.LogError(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"T{{es}}t");
        Logger.LogError(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogError_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");
        Logger.LogError(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value,3}");
        Logger.LogError(exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value:N}");
        Logger.LogError(exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value,8:N}");
        Logger.LogError(exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogError_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");
        Logger.LogError(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogError_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");
        Logger.LogError(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");
        Logger.LogError(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_Mixed_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogError(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogError_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogError(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogError_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"Test");
        Logger.LogError(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"T{{es}}t");
        Logger.LogError(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogError_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");
        Logger.LogError(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value,3}");
        Logger.LogError(eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value:N}");
        Logger.LogError(eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value,8:N}");
        Logger.LogError(eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogError_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");
        Logger.LogError(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogError_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");
        Logger.LogError(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");
        Logger.LogError(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_Mixed_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.LogError(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogError(eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogError_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogError(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogError(eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogError_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"Test");
        Logger.LogError(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"T{{es}}t");
        Logger.LogError(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogError_Scalar_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value}");
        Logger.LogError(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value,3}");
        Logger.LogError(eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value:N}");
        Logger.LogError(eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value,8:N}");
        Logger.LogError(eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogError_Complex_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value}");
        Logger.LogError(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogError_Collection_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value}");
        Logger.LogError(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_String_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{value}");
        Logger.LogError(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogError_Mixed_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogError(eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogError_Named_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogError(eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // LogError


    #region LogCritical


    [Test]
    public void LogCritical_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        
        
        Logger.LogCritical($"Test");
        Logger.LogCritical(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "T{{es}}t";

        
        
        Logger.LogCritical($"T{{es}}t");
        Logger.LogCritical(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogCritical_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        
        Logger.LogCritical($"{value}");
        Logger.LogCritical("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        
        Logger.LogCritical($"{value,3}");
        Logger.LogCritical("{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        
        Logger.LogCritical($"{value:N}");
        Logger.LogCritical("{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        
        Logger.LogCritical($"{value,8:N}");
        Logger.LogCritical("{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogCritical_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        
        Logger.LogCritical($"{value}");
        Logger.LogCritical("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogCritical_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogCritical($"{value}");
        Logger.LogCritical("{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_String()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        
        
        Logger.LogCritical($"{value}");
        Logger.LogCritical("{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_Mixed()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.LogCritical(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogCritical(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogCritical_Named()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        
        Logger.LogCritical(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogCritical(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogCritical_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"Test");
        Logger.LogCritical(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"T{{es}}t");
        Logger.LogCritical(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogCritical_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");
        Logger.LogCritical(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value,3}");
        Logger.LogCritical(exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value:N}");
        Logger.LogCritical(exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value,8:N}");
        Logger.LogCritical(exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogCritical_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");
        Logger.LogCritical(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogCritical_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");
        Logger.LogCritical(exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");
        Logger.LogCritical(exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_Mixed_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogCritical(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogCritical_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogCritical(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogCritical_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"Test");
        Logger.LogCritical(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"T{{es}}t");
        Logger.LogCritical(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogCritical_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");
        Logger.LogCritical(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value,3}");
        Logger.LogCritical(eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value:N}");
        Logger.LogCritical(eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value,8:N}");
        Logger.LogCritical(eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogCritical_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");
        Logger.LogCritical(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogCritical_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");
        Logger.LogCritical(eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");
        Logger.LogCritical(eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_Mixed_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.LogCritical(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogCritical(eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogCritical_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogCritical(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogCritical(eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void LogCritical_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"Test");
        Logger.LogCritical(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"T{{es}}t");
        Logger.LogCritical(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void LogCritical_Scalar_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value}");
        Logger.LogCritical(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value,3}");
        Logger.LogCritical(eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value:N}");
        Logger.LogCritical(eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value,8:N}");
        Logger.LogCritical(eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void LogCritical_Complex_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value}");
        Logger.LogCritical(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void LogCritical_Collection_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value}");
        Logger.LogCritical(eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_String_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{value}");
        Logger.LogCritical(eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void LogCritical_Mixed_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.LogCritical(eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void LogCritical_Named_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.LogCritical(eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // LogCritical


    #region Log


    [Test]
    public void Log_Literal([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        
        
        Logger.Log(logLevel, $"Test");
        Logger.Log(logLevel, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_LiteralWithBraces([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "T{{es}}t";

        
        
        Logger.Log(logLevel, $"T{{es}}t");
        Logger.Log(logLevel, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void Log_Scalar([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        
        Logger.Log(logLevel, $"{value}");
        Logger.Log(logLevel, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        
        Logger.Log(logLevel, $"{value,3}");
        Logger.Log(logLevel, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void Log_ScalarWithFormat([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        
        Logger.Log(logLevel, $"{value:N}");
        Logger.Log(logLevel, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        
        Logger.Log(logLevel, $"{value,8:N}");
        Logger.Log(logLevel, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void Log_Complex([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        
        Logger.Log(logLevel, $"{value}");
        Logger.Log(logLevel, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void Log_Collection([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        
        
        Logger.Log(logLevel, $"{value}");
        Logger.Log(logLevel, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_String([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        
        
        Logger.Log(logLevel, $"{value}");
        Logger.Log(logLevel, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_Mixed([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        
        Logger.Log(logLevel,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Log(logLevel,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void Log_Named([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        
        Logger.Log(logLevel,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Log(logLevel,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void Log_Literal_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"Test");
        Logger.Log(logLevel, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_LiteralWithBraces_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "T{{es}}t";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"T{{es}}t");
        Logger.Log(logLevel, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void Log_Scalar_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");
        Logger.Log(logLevel, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value,3}");
        Logger.Log(logLevel, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void Log_ScalarWithFormat_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value:N}");
        Logger.Log(logLevel, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value,8:N}");
        Logger.Log(logLevel, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void Log_Complex_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");
        Logger.Log(logLevel, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void Log_Collection_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");
        Logger.Log(logLevel, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_String_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");
        Logger.Log(logLevel, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_Mixed_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Log(logLevel, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void Log_Named_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Log(logLevel, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void Log_Literal_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"Test");
        Logger.Log(logLevel, eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_LiteralWithBraces_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "T{{es}}t";

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"T{{es}}t");
        Logger.Log(logLevel, eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void Log_Scalar_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");
        Logger.Log(logLevel, eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value,3}");
        Logger.Log(logLevel, eventId, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void Log_ScalarWithFormat_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value:N}");
        Logger.Log(logLevel, eventId, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value,8:N}");
        Logger.Log(logLevel, eventId, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void Log_Complex_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");
        Logger.Log(logLevel, eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void Log_Collection_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");
        Logger.Log(logLevel, eventId, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_String_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");
        Logger.Log(logLevel, eventId, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_Mixed_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Log(logLevel, eventId,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void Log_Named_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Log(logLevel, eventId,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    default(Exception),
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    [Test]
    public void Log_Literal_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"Test");
        Logger.Log(logLevel, eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_LiteralWithBraces_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "T{{es}}t";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"T{{es}}t");
        Logger.Log(logLevel, eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse(message),
                    new LogEventProperty[]
                    {
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void Log_Scalar_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");
        Logger.Log(logLevel, eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value,3}");
        Logger.Log(logLevel, eventId, exception, "{value,3}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,3}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value,3));
        }
    }

    [Test]
    public void Log_ScalarWithFormat_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value:N}");
        Logger.Log(logLevel, eventId, exception, "{value:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value,8:N}");
        Logger.Log(logLevel, eventId, exception, "{value,8:N}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value,8:N}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value, 8, "N"));
        }
    }

    [Test]
    public void Log_Complex_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");
        Logger.Log(logLevel, eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));      
        }
    }

    [Test]
    public void Log_Collection_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");
        Logger.Log(logLevel, eventId, exception, "{@value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{@value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_String_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");
        Logger.Log(logLevel, eventId, exception, "{value}", value);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("{value}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(Format(value));
        }
    }

    [Test]
    public void Log_Mixed_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Log(logLevel, eventId, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                    new LogEventProperty[]
                    {
                        arg1.ToLogEventProperty(),
                        arg2.ToLogEventProperty(),
                        arg3.ToLogEventProperty(),
                        arg4.ToLogEventProperty(),
                        arg5.ToLogEventProperty(),
                        veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventProperty(),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");
        }
    }

    [Test]
    public void Log_Named_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Log(logLevel, eventId, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);


        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.Should().BeEquivalentTo(
                new LogEvent(
                    DateTimeOffset.Now,
                    ToSerilogLevel(logLevel),
                    exception,
                    Parser.Parse("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}"),
                    new LogEventProperty[]
                    {
                        value.ToLogEventProperty("default"),
                        value.ToString().ToLogEventProperty("str"),
                        value.ToLogEventProperty("destructured"),
                        SourceContextProperty,
                        EventIdProperty(eventId)
                    }
                ),
                LogEventEquivalency);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(
                $"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");     
        }
    }


    #endregion // Log


}