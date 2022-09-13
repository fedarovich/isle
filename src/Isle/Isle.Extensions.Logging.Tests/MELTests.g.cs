#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using FluentAssertions;

namespace Isle.Extensions.Logging.Tests;

[TestFixtureSource(nameof(FixtureArgs))]
public class LoggerExtensionsTests : BaseFixture
{
    public LoggerExtensionsTests(LogLevel minLogLevel, bool enableCaching) : base(minLogLevel, enableCaching)
    {
    }


    #region LogTrace


    [Test]
    public void LogTrace_Literal()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        
        Logger.LogTrace($"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        
        Logger.LogTrace($"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogTrace_MixedWithLiteralLists()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.LogTrace(            $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_Named()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        
        Logger.LogTrace(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogTrace_LiteralValue([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        
        Logger.LogTrace($"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        
        Logger.LogTrace($"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_MixedLiteralValue([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Trace;
        
        
        Logger.LogTrace(            $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogTrace_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogTrace_MixedWithLiteralLists_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogTrace_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Trace;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogTrace_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogTrace_MixedWithLiteralLists_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.LogTrace(eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogTrace(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogTrace_LiteralValue_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralValueWithBraces_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        
        Logger.LogTrace(eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_MixedLiteralValue_WithEventId([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        
        Logger.LogTrace(eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogTrace_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogTrace_MixedWithLiteralLists_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
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

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogTrace_LiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_LiteralValueWithBraces_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogTrace_MixedLiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Trace;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogTrace(eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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
        
        
        Logger.LogDebug($"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        
        Logger.LogDebug($"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogDebug_MixedWithLiteralLists()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.LogDebug(            $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_Named()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        
        Logger.LogDebug(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogDebug_LiteralValue([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        
        Logger.LogDebug($"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        
        Logger.LogDebug($"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_MixedLiteralValue([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Debug;
        
        
        Logger.LogDebug(            $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogDebug_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogDebug_MixedWithLiteralLists_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogDebug_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Debug;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogDebug_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogDebug_MixedWithLiteralLists_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.LogDebug(eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogDebug(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogDebug_LiteralValue_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralValueWithBraces_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        
        Logger.LogDebug(eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_MixedLiteralValue_WithEventId([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        
        Logger.LogDebug(eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogDebug_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogDebug_MixedWithLiteralLists_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
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

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogDebug_LiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_LiteralValueWithBraces_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogDebug_MixedLiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Debug;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogDebug(eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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
        
        
        Logger.LogInformation($"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        
        Logger.LogInformation($"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogInformation_MixedWithLiteralLists()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.LogInformation(            $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_Named()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        
        Logger.LogInformation(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogInformation_LiteralValue([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        
        Logger.LogInformation($"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        
        Logger.LogInformation($"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_MixedLiteralValue([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Information;
        
        
        Logger.LogInformation(            $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogInformation_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogInformation_MixedWithLiteralLists_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogInformation_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Information;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogInformation_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogInformation_MixedWithLiteralLists_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.LogInformation(eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogInformation(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogInformation_LiteralValue_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralValueWithBraces_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        
        Logger.LogInformation(eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_MixedLiteralValue_WithEventId([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        
        Logger.LogInformation(eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogInformation_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogInformation_MixedWithLiteralLists_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Information;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
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

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogInformation_LiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_LiteralValueWithBraces_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogInformation_MixedLiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Information;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogInformation(eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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
        
        
        Logger.LogWarning($"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        
        Logger.LogWarning($"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogWarning_MixedWithLiteralLists()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.LogWarning(            $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_Named()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        
        Logger.LogWarning(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogWarning_LiteralValue([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        
        Logger.LogWarning($"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        
        Logger.LogWarning($"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_MixedLiteralValue([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Warning;
        
        
        Logger.LogWarning(            $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogWarning_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogWarning_MixedWithLiteralLists_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogWarning_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Warning;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogWarning_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogWarning_MixedWithLiteralLists_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.LogWarning(eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogWarning(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogWarning_LiteralValue_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralValueWithBraces_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        
        Logger.LogWarning(eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_MixedLiteralValue_WithEventId([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        
        Logger.LogWarning(eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogWarning_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogWarning_MixedWithLiteralLists_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
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

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogWarning_LiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_LiteralValueWithBraces_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogWarning_MixedLiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Warning;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogWarning(eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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
        
        
        Logger.LogError($"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        
        Logger.LogError($"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogError_MixedWithLiteralLists()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.LogError(            $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_Named()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        
        Logger.LogError(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogError_LiteralValue([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        
        Logger.LogError($"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        
        Logger.LogError($"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_MixedLiteralValue([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Error;
        
        
        Logger.LogError(            $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogError_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogError_MixedWithLiteralLists_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogError_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Error;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogError_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        
        Logger.LogError(eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        
        Logger.LogError(eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogError_MixedWithLiteralLists_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.LogError(eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogError(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogError_LiteralValue_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralValueWithBraces_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        
        Logger.LogError(eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_MixedLiteralValue_WithEventId([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        
        Logger.LogError(eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogError_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogError_MixedWithLiteralLists_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Error;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
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

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogError_LiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_LiteralValueWithBraces_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogError_MixedLiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Error;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogError(eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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
        
        
        Logger.LogCritical($"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        
        Logger.LogCritical($"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogCritical_MixedWithLiteralLists()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.LogCritical(            $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_Named()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        
        Logger.LogCritical(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogCritical_LiteralValue([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        
        Logger.LogCritical($"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        
        Logger.LogCritical($"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_MixedLiteralValue([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Critical;
        
        
        Logger.LogCritical(            $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogCritical_Literal_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogCritical_MixedWithLiteralLists_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_Named_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogCritical_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Critical;
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogCritical_Literal_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogCritical_MixedWithLiteralLists_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.LogCritical(eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_Named_WithEventId()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.LogCritical(eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogCritical_LiteralValue_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralValueWithBraces_WithEventId([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        
        Logger.LogCritical(eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_MixedLiteralValue_WithEventId([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        
        Logger.LogCritical(eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void LogCritical_Literal_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralWithBraces_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void LogCritical_MixedWithLiteralLists_WithEventId_WithException()
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
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

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void LogCritical_LiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_LiteralValueWithBraces_WithEventId_WithException([Values] bool cacheable)
    {
 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void LogCritical_MixedLiteralValue_WithEventId_WithException([Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

 
        const LogLevel logLevel = LogLevel.Critical;
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.LogCritical(eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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
        
        
        Logger.Log(logLevel, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralWithBraces([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        
        
        Logger.Log(logLevel, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void Log_MixedWithLiteralLists([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        
        Logger.Log(logLevel,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_Named([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        
        Logger.Log(logLevel,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void Log_LiteralValue([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        
        
        Logger.Log(logLevel, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralValueWithBraces([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        
        
        Logger.Log(logLevel, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_MixedLiteralValue([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

        
        
        Logger.Log(logLevel,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void Log_Literal_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralWithBraces_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void Log_MixedWithLiteralLists_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_Named_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void Log_LiteralValue_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralValueWithBraces_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_MixedLiteralValue_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

        
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void Log_Literal_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralWithBraces_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void Log_MixedWithLiteralLists_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_Named_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void Log_LiteralValue_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralValueWithBraces_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_MixedLiteralValue_WithEventId([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

        EventId eventId = 5;
        
        Logger.Log(logLevel, eventId,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }


    [Test]
    public void Log_Literal_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"Test");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralWithBraces_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"T{{es}}t");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
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
    public void Log_MixedWithLiteralLists_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var arg1 = 5;
        var arg2 = new TestObject(7, 11);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception,             $"A" + $"B" + $"C" + $"{arg1}" + $"D" + $"E" + $"F{arg2}G" + $"H" + $"" + $"I{{J}}" + $"K");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format("ABC{0}DEF{1}GHI{{J}}K", arg1, arg2)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("arg1", arg1),
                new KeyValuePair<string, object>("@arg2", arg2),
                new KeyValuePair<string, object>("{OriginalFormat}", "ABC{arg1}DEF{@arg2}GHI{{J}}K")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_Named_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel)
    {
        var value = new TestObject(3, 5);

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = string.Format(CultureInfo.InvariantCulture, "Default: {0}, Stringified: {0}, Destructured: {0}", value)
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("@default", value),
                new KeyValuePair<string, object>("$str", value),
                new KeyValuePair<string, object>("@destructured", value),
                new KeyValuePair<string, object>("{OriginalFormat}", "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}")
            });
            logItem.Scopes.Should().BeEmpty();            
        }
    }
    
    [Test]
    public void Log_LiteralValue_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{new LiteralValue("Test", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "Test"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "Test")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_LiteralValueWithBraces_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception, $"{new LiteralValue("T{es}t", cacheable)}");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "T{es}t"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("{OriginalFormat}", "T{{es}}t")
            });
            logItem.Scopes.Should().BeEmpty();
        }
    }

    [Test]
    public void Log_MixedLiteralValue_WithEventId_WithException([ValueSource(nameof(LogLevels))] LogLevel logLevel, [Values] bool cacheable)
    {
        var x = 1;
        var y = 2;

        EventId eventId = 5;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.Log(logLevel, eventId, exception,             $"A{x}B{new LiteralValue("C", cacheable)}D{y}E");

        if (logLevel < MinLogLevel)
        {
            LogItems.Should().BeEmpty();
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
                Message = "A1BCD2E"
            });
            logItem.State.Should().BeEquivalentTo(new[]
            {
                new KeyValuePair<string, object>("x", x),
                new KeyValuePair<string, object>("y", y),
                new KeyValuePair<string, object>("{OriginalFormat}", "A{x}BCD{y}E")
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