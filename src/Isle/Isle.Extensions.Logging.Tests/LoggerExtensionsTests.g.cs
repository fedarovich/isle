#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Logging;
using Isle.Configuration;
using NUnit.Framework;
using FluentAssertions;

namespace Isle.Extensions.Logging.Tests;

[TestFixtureSource(nameof(FixtureArgs))]
public class LoggerExtensionsTests : BaseFixture
{
    public LoggerExtensionsTests(LogLevel minLogLevel) : base(minLogLevel)
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogTrace_Scopes()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.LogTrace($"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogDebug_Scopes()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.LogDebug($"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogInformation_Scopes()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.LogInformation($"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogWarning_Scopes()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.LogWarning($"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogError_Scopes()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.LogError($"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogCritical_Scopes()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.LogCritical($"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_Scalar([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_ScalarWithAlignment([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;

        
        
        Logger.Log(logLevel, $"{value,3}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_ScalarWithFormat([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        
        Logger.Log(logLevel, $"{value:N}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_ScalarWithAlignmentAndFormat([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7000;

        
        
        Logger.Log(logLevel, $"{value,8:N}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_Complex([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }

    [Test]
    public void Log_Collection([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new int[] {1, 2, 3};

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_String([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = "Test";

        
        
        Logger.Log(logLevel, $"{value}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = default(Exception),
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = message
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", message)
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,3}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,3}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0,8:N}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value,8:N}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();            
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = "1, 2, 3"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{@value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = value
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEmpty();
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
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = eventId,
                Exception = exception,
                Message = string.Format("ABCD{0,7}EFGH{1:N}IJKL{2,10:F3}MNOP{3}QRST{4}UVWX{5}YZ",
                    arg1, arg2, arg3, arg4, "5, 4, 3", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("arg2", arg2),
                new KeyValuePair<string, object>("arg3", arg3),
                new KeyValuePair<string, object>("@arg4", arg4),
                new KeyValuePair<string, object>("@arg5", arg5),
                new KeyValuePair<string, object>("veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg", veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void Log_Scopes([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        int value = 7;
        int scope1 = 3;
        var scope2 = new TestObject(5, 11);

        using (Logger.BeginScopeInterpolated($"Scope 1 value: {scope1}"))
        {
            using (Logger.BeginScopeInterpolated($"Scope 2 value: {scope2}"))
            {
                Logger.Log(logLevel, $"{value}");
            }
        }

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
        }
        else
        {
            LogItems.Should().HaveCount(1);
            var logItem = LogItems.Single();
            logItem.Should().BeEquivalentTo(new
            {
                Category = GetType().FullName,
                LogLevel = logLevel,
                EventId = default(EventId),
                Exception = default(Exception),
                Message = string.Format(CultureInfo.InvariantCulture, "{0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("value", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "{value}")
            });
            logItem.Scopes.Should().BeEquivalentTo(new[]
            {
                new[] 
                {
                    new KeyValuePair<string, object>("@scope2", scope2),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 2 value: {@scope2}")
                },
                new[] 
                {
                    new KeyValuePair<string, object>("scope1", scope1),
                    new KeyValuePair<string, object>("{OriginalFormat}", "Scope 1 value: {scope1}")
                }
            });
        }
    }

    #endregion // Log


}