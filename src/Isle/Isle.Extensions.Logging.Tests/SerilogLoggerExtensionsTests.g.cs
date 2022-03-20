﻿#nullable enable
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
    public SerilogLoggerExtensionsTests(LogLevel minLogLevel) : base(minLogLevel)
    {
    }


    #region LogTrace


    [Test]
    public void LogTrace_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        
        
        Logger.LogTrace(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        
        Logger.LogTrace($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        
        Logger.LogTrace($"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        
        Logger.LogTrace($"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        
        Logger.LogTrace($"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogTrace_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        
        Logger.LogTrace($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogTrace_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogTrace($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogTrace_String()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        
        
        Logger.LogTrace($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.LogTrace(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogTrace_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogTrace_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogTrace_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogTrace_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogTrace(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogTrace_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogTrace_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogTrace_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogTrace_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogTrace_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogTrace_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.LogTrace(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogTrace_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogTrace(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // LogTrace


    #region LogDebug


    [Test]
    public void LogDebug_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        
        
        Logger.LogDebug(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        
        Logger.LogDebug($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        
        Logger.LogDebug($"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        
        Logger.LogDebug($"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        
        Logger.LogDebug($"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogDebug_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        
        Logger.LogDebug($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogDebug_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogDebug($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogDebug_String()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        
        
        Logger.LogDebug($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.LogDebug(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogDebug_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogDebug_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogDebug_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogDebug_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogDebug(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogDebug_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogDebug_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogDebug_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogDebug_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogDebug_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogDebug_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.LogDebug(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogDebug_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogDebug(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // LogDebug


    #region LogInformation


    [Test]
    public void LogInformation_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        
        
        Logger.LogInformation(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        
        Logger.LogInformation($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        
        Logger.LogInformation($"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        
        Logger.LogInformation($"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        
        Logger.LogInformation($"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogInformation_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        
        Logger.LogInformation($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogInformation_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogInformation($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogInformation_String()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        
        
        Logger.LogInformation($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.LogInformation(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogInformation_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogInformation_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogInformation_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogInformation_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogInformation(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogInformation_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogInformation_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogInformation_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogInformation_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogInformation_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogInformation_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.LogInformation(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogInformation_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogInformation(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // LogInformation


    #region LogWarning


    [Test]
    public void LogWarning_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        
        
        Logger.LogWarning(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        
        Logger.LogWarning($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        
        Logger.LogWarning($"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        
        Logger.LogWarning($"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        
        Logger.LogWarning($"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogWarning_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        
        Logger.LogWarning($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogWarning_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogWarning($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogWarning_String()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        
        
        Logger.LogWarning($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.LogWarning(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogWarning_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogWarning_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogWarning_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogWarning_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogWarning(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogWarning_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogWarning_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogWarning_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogWarning_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogWarning_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogWarning_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.LogWarning(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogWarning_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogWarning(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // LogWarning


    #region LogError


    [Test]
    public void LogError_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        
        
        Logger.LogError(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        
        Logger.LogError($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        
        Logger.LogError($"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        
        Logger.LogError($"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        
        Logger.LogError($"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogError_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        
        Logger.LogError($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogError_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogError($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogError_String()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        
        
        Logger.LogError($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.LogError(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogError_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogError_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogError_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogError_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogError(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogError_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogError(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogError_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogError_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogError_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogError_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogError_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.LogError(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogError_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogError(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // LogError


    #region LogCritical


    [Test]
    public void LogCritical_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        
        
        Logger.LogCritical(message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_Scalar()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        
        Logger.LogCritical($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        
        Logger.LogCritical($"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        
        Logger.LogCritical($"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        
        Logger.LogCritical($"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogCritical_Complex()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        
        Logger.LogCritical($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogCritical_Collection()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        
        
        Logger.LogCritical($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogCritical_String()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        
        
        Logger.LogCritical($"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.LogCritical(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogCritical_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_Scalar_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogCritical_Complex_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogCritical_Collection_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogCritical_String_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogCritical(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogCritical_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void LogCritical_Scalar_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignment_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void LogCritical_ScalarWithAlignmentAndFormat_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7000;

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void LogCritical_Complex_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void LogCritical_Collection_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void LogCritical_String_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = "Test";

        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.LogCritical(eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void LogCritical_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
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

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.LogCritical(eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // LogCritical


    #region Log


    [Test]
    public void Log_Literal([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        
        
        Logger.Log(logLevel, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_Scalar([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        
        Logger.Log(logLevel, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void Log_ScalarWithFormat([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        
        Logger.Log(logLevel, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        
        Logger.Log(logLevel, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void Log_Complex([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void Log_Collection([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void Log_String([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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

        
        
        Logger.Log(logLevel,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void Log_Literal_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_Scalar_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void Log_ScalarWithFormat_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void Log_Complex_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void Log_Collection_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void Log_String_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.Log(logLevel, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void Log_Literal_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_Scalar_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void Log_ScalarWithFormat_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void Log_Complex_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void Log_Collection_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void Log_String_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        
        Logger.Log(logLevel, eventId,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = default(Exception),
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    [Test]
    public void Log_Literal_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        const string message = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, message);

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse(message),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void Log_Scalar_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignment_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,3}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,3}", value));
        }
    }

    [Test]
    public void Log_ScalarWithFormat_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0:N}", value));
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value,8:N}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value));
        }
    }

    [Test]
    public void Log_Complex_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "TestObject {{ X: {0}, Y: {1} }}", value.X, value.Y));          
        }
    }

    [Test]
    public void Log_Collection_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{@value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "[1, 2, 3]", value));
        }
    }

    [Test]
    public void Log_String_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("{value}"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["value"] = new ScalarValue(value),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, "\"{0}\"", value));
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
        Logger.Log(logLevel, eventId, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");

        if (logLevel < MinLogLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.Should().BeEquivalentTo(new 
            {
                Exception = exception,
                Level = ToSerilogLevel(logLevel),
                MessageTemplate = Parser.Parse("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ"),
                Properties = new Dictionary<string, LogEventPropertyValue>
                {
                    ["arg1"] = new ScalarValue(arg1),
                    ["arg2"] = new ScalarValue(arg2),
                    ["arg3"] = new ScalarValue(arg3),
                    ["arg4"] = new ScalarValue(arg4),
                    ["arg5"] = new ScalarValue(arg5),
                    ["veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg"] = new ScalarValue(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                    ["SourceContext"] = new ScalarValue(GetType().FullName),
                    ["EventId"] = new ScalarValue(eventId)
                }
            });
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(string.Format(CultureInfo.InvariantCulture, 
                "ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                "\"" + arg1 + "\"", 
                arg2, 
                arg3, 
                "TestObject { X: 7, Y: 11 }", 
                "[5, 4, 3]",
                "\"" + veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg + "\""));
        }
    }


    #endregion // Log


}