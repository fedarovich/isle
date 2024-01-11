#nullable enable
using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Serilog.Events;
using Serilog.Parsing;
using Isle.Extensions;

namespace Isle.Serilog.Tests;

[TestFixtureSource(nameof(FixtureArgs))]
public class LoggerExtensionsTests : BaseFixture
{
    public LoggerExtensionsTests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching)
    {
    }


    #region Verbose

    [Test]
    public void VerboseInterpolated_Literal()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "Test";

        
        Logger.VerboseInterpolated($"Test");
        Logger.Verbose(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void VerboseInterpolated_LiteralWithBraces()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "T{{es}}t";

        
        Logger.VerboseInterpolated($"T{{es}}t");
        Logger.Verbose(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void VerboseInterpolated_Scalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        
        Logger.VerboseInterpolated($"{value}");
        Logger.Verbose("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void VerboseInterpolated_ScalarWithAlignment()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        
        Logger.VerboseInterpolated($"{value,3}");
        Logger.Verbose("{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void VerboseInterpolated_ScalarWithFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        
        Logger.VerboseInterpolated($"{value:N}");
        Logger.Verbose("{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void VerboseInterpolated_ScalarWithAlignmentAndFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        
        Logger.VerboseInterpolated($"{value,6:N}");
        Logger.Verbose("{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void VerboseInterpolated_Complex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = new TestObject(3, 5);

        
        Logger.VerboseInterpolated($"{value}");
        Logger.Verbose("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void VerboseInterpolated_Collection()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = new int[] {1, 2, 3};

        
        Logger.VerboseInterpolated($"{value}");
        Logger.Verbose("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void VerboseInterpolated_String()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        string value = "Test";

        
        Logger.VerboseInterpolated($"{value}");
        Logger.Verbose("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void VerboseInterpolated_Mixed()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.VerboseInterpolated(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Verbose(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void VerboseInterpolated_NamedScalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = 42;

        
        Logger.VerboseInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Verbose(            "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void VerboseInterpolated_NamedComplex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = new TestObject(3, 5);

        
        Logger.VerboseInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Verbose(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void VerboseInterpolated_LiteralValue([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "Test";

        
        Logger.VerboseInterpolated($"{new LiteralValue("Test", cacheable)}");
        Logger.Verbose(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void VerboseInterpolated_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "T{{es}}t";

        
        Logger.VerboseInterpolated($"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Verbose(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void VerboseInterpolated_MixedLiteralValue([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        
        Logger.VerboseInterpolated($"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void VerboseInterpolated_Literal_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"Test");
        Logger.Verbose(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void VerboseInterpolated_LiteralWithBraces_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"T{{es}}t");
        Logger.Verbose(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void VerboseInterpolated_Scalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value}");
        Logger.Verbose(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void VerboseInterpolated_ScalarWithAlignment_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value,3}");
        Logger.Verbose(exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void VerboseInterpolated_ScalarWithFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value:N}");
        Logger.Verbose(exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void VerboseInterpolated_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value,6:N}");
        Logger.Verbose(exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void VerboseInterpolated_Complex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value}");
        Logger.Verbose(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void VerboseInterpolated_Collection_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value}");
        Logger.Verbose(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void VerboseInterpolated_String_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{value}");
        Logger.Verbose(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void VerboseInterpolated_Mixed_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Verbose(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void VerboseInterpolated_NamedScalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Verbose(exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void VerboseInterpolated_NamedComplex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Verbose(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void VerboseInterpolated_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Verbose(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void VerboseInterpolated_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Verbose(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void VerboseInterpolated_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Verbose;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.VerboseInterpolated(exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Verbose);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion

    #region Debug

    [Test]
    public void DebugInterpolated_Literal()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "Test";

        
        Logger.DebugInterpolated($"Test");
        Logger.Debug(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void DebugInterpolated_LiteralWithBraces()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "T{{es}}t";

        
        Logger.DebugInterpolated($"T{{es}}t");
        Logger.Debug(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void DebugInterpolated_Scalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        
        Logger.DebugInterpolated($"{value}");
        Logger.Debug("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void DebugInterpolated_ScalarWithAlignment()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        
        Logger.DebugInterpolated($"{value,3}");
        Logger.Debug("{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void DebugInterpolated_ScalarWithFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        
        Logger.DebugInterpolated($"{value:N}");
        Logger.Debug("{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void DebugInterpolated_ScalarWithAlignmentAndFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        
        Logger.DebugInterpolated($"{value,6:N}");
        Logger.Debug("{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void DebugInterpolated_Complex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = new TestObject(3, 5);

        
        Logger.DebugInterpolated($"{value}");
        Logger.Debug("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void DebugInterpolated_Collection()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = new int[] {1, 2, 3};

        
        Logger.DebugInterpolated($"{value}");
        Logger.Debug("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void DebugInterpolated_String()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        string value = "Test";

        
        Logger.DebugInterpolated($"{value}");
        Logger.Debug("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void DebugInterpolated_Mixed()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.DebugInterpolated(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Debug(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void DebugInterpolated_NamedScalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = 42;

        
        Logger.DebugInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Debug(            "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void DebugInterpolated_NamedComplex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = new TestObject(3, 5);

        
        Logger.DebugInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Debug(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void DebugInterpolated_LiteralValue([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "Test";

        
        Logger.DebugInterpolated($"{new LiteralValue("Test", cacheable)}");
        Logger.Debug(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void DebugInterpolated_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "T{{es}}t";

        
        Logger.DebugInterpolated($"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Debug(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void DebugInterpolated_MixedLiteralValue([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        
        Logger.DebugInterpolated($"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void DebugInterpolated_Literal_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"Test");
        Logger.Debug(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void DebugInterpolated_LiteralWithBraces_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"T{{es}}t");
        Logger.Debug(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void DebugInterpolated_Scalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value}");
        Logger.Debug(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void DebugInterpolated_ScalarWithAlignment_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value,3}");
        Logger.Debug(exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void DebugInterpolated_ScalarWithFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value:N}");
        Logger.Debug(exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void DebugInterpolated_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value,6:N}");
        Logger.Debug(exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void DebugInterpolated_Complex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value}");
        Logger.Debug(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void DebugInterpolated_Collection_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value}");
        Logger.Debug(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void DebugInterpolated_String_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{value}");
        Logger.Debug(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void DebugInterpolated_Mixed_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Debug(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void DebugInterpolated_NamedScalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Debug(exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void DebugInterpolated_NamedComplex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Debug(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void DebugInterpolated_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Debug(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void DebugInterpolated_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Debug(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void DebugInterpolated_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Debug;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.DebugInterpolated(exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Debug);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion

    #region Information

    [Test]
    public void InformationInterpolated_Literal()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "Test";

        
        Logger.InformationInterpolated($"Test");
        Logger.Information(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void InformationInterpolated_LiteralWithBraces()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "T{{es}}t";

        
        Logger.InformationInterpolated($"T{{es}}t");
        Logger.Information(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void InformationInterpolated_Scalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        
        Logger.InformationInterpolated($"{value}");
        Logger.Information("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void InformationInterpolated_ScalarWithAlignment()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        
        Logger.InformationInterpolated($"{value,3}");
        Logger.Information("{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void InformationInterpolated_ScalarWithFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        
        Logger.InformationInterpolated($"{value:N}");
        Logger.Information("{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void InformationInterpolated_ScalarWithAlignmentAndFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        
        Logger.InformationInterpolated($"{value,6:N}");
        Logger.Information("{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void InformationInterpolated_Complex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = new TestObject(3, 5);

        
        Logger.InformationInterpolated($"{value}");
        Logger.Information("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void InformationInterpolated_Collection()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = new int[] {1, 2, 3};

        
        Logger.InformationInterpolated($"{value}");
        Logger.Information("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void InformationInterpolated_String()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        string value = "Test";

        
        Logger.InformationInterpolated($"{value}");
        Logger.Information("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void InformationInterpolated_Mixed()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.InformationInterpolated(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Information(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void InformationInterpolated_NamedScalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = 42;

        
        Logger.InformationInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Information(            "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void InformationInterpolated_NamedComplex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = new TestObject(3, 5);

        
        Logger.InformationInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Information(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void InformationInterpolated_LiteralValue([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "Test";

        
        Logger.InformationInterpolated($"{new LiteralValue("Test", cacheable)}");
        Logger.Information(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void InformationInterpolated_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "T{{es}}t";

        
        Logger.InformationInterpolated($"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Information(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void InformationInterpolated_MixedLiteralValue([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        
        Logger.InformationInterpolated($"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void InformationInterpolated_Literal_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"Test");
        Logger.Information(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void InformationInterpolated_LiteralWithBraces_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"T{{es}}t");
        Logger.Information(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void InformationInterpolated_Scalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value}");
        Logger.Information(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void InformationInterpolated_ScalarWithAlignment_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value,3}");
        Logger.Information(exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void InformationInterpolated_ScalarWithFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value:N}");
        Logger.Information(exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void InformationInterpolated_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value,6:N}");
        Logger.Information(exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void InformationInterpolated_Complex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value}");
        Logger.Information(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void InformationInterpolated_Collection_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value}");
        Logger.Information(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void InformationInterpolated_String_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{value}");
        Logger.Information(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void InformationInterpolated_Mixed_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Information(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void InformationInterpolated_NamedScalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Information(exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void InformationInterpolated_NamedComplex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Information(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void InformationInterpolated_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Information(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void InformationInterpolated_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Information(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void InformationInterpolated_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Information;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.InformationInterpolated(exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Information);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion

    #region Warning

    [Test]
    public void WarningInterpolated_Literal()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "Test";

        
        Logger.WarningInterpolated($"Test");
        Logger.Warning(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WarningInterpolated_LiteralWithBraces()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "T{{es}}t";

        
        Logger.WarningInterpolated($"T{{es}}t");
        Logger.Warning(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WarningInterpolated_Scalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        
        Logger.WarningInterpolated($"{value}");
        Logger.Warning("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WarningInterpolated_ScalarWithAlignment()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        
        Logger.WarningInterpolated($"{value,3}");
        Logger.Warning("{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void WarningInterpolated_ScalarWithFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        
        Logger.WarningInterpolated($"{value:N}");
        Logger.Warning("{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void WarningInterpolated_ScalarWithAlignmentAndFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        
        Logger.WarningInterpolated($"{value,6:N}");
        Logger.Warning("{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void WarningInterpolated_Complex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = new TestObject(3, 5);

        
        Logger.WarningInterpolated($"{value}");
        Logger.Warning("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void WarningInterpolated_Collection()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = new int[] {1, 2, 3};

        
        Logger.WarningInterpolated($"{value}");
        Logger.Warning("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WarningInterpolated_String()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        string value = "Test";

        
        Logger.WarningInterpolated($"{value}");
        Logger.Warning("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WarningInterpolated_Mixed()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.WarningInterpolated(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Warning(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void WarningInterpolated_NamedScalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = 42;

        
        Logger.WarningInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Warning(            "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WarningInterpolated_NamedComplex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = new TestObject(3, 5);

        
        Logger.WarningInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Warning(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WarningInterpolated_LiteralValue([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "Test";

        
        Logger.WarningInterpolated($"{new LiteralValue("Test", cacheable)}");
        Logger.Warning(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WarningInterpolated_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "T{{es}}t";

        
        Logger.WarningInterpolated($"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Warning(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WarningInterpolated_MixedLiteralValue([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        
        Logger.WarningInterpolated($"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void WarningInterpolated_Literal_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"Test");
        Logger.Warning(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WarningInterpolated_LiteralWithBraces_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"T{{es}}t");
        Logger.Warning(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WarningInterpolated_Scalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value}");
        Logger.Warning(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WarningInterpolated_ScalarWithAlignment_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value,3}");
        Logger.Warning(exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void WarningInterpolated_ScalarWithFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value:N}");
        Logger.Warning(exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void WarningInterpolated_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value,6:N}");
        Logger.Warning(exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void WarningInterpolated_Complex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value}");
        Logger.Warning(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void WarningInterpolated_Collection_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value}");
        Logger.Warning(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WarningInterpolated_String_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{value}");
        Logger.Warning(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WarningInterpolated_Mixed_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Warning(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void WarningInterpolated_NamedScalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Warning(exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WarningInterpolated_NamedComplex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Warning(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WarningInterpolated_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Warning(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WarningInterpolated_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Warning(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WarningInterpolated_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Warning;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WarningInterpolated(exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Warning);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion

    #region Error

    [Test]
    public void ErrorInterpolated_Literal()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "Test";

        
        Logger.ErrorInterpolated($"Test");
        Logger.Error(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void ErrorInterpolated_LiteralWithBraces()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "T{{es}}t";

        
        Logger.ErrorInterpolated($"T{{es}}t");
        Logger.Error(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void ErrorInterpolated_Scalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        
        Logger.ErrorInterpolated($"{value}");
        Logger.Error("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void ErrorInterpolated_ScalarWithAlignment()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        
        Logger.ErrorInterpolated($"{value,3}");
        Logger.Error("{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void ErrorInterpolated_ScalarWithFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        
        Logger.ErrorInterpolated($"{value:N}");
        Logger.Error("{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void ErrorInterpolated_ScalarWithAlignmentAndFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        
        Logger.ErrorInterpolated($"{value,6:N}");
        Logger.Error("{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void ErrorInterpolated_Complex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = new TestObject(3, 5);

        
        Logger.ErrorInterpolated($"{value}");
        Logger.Error("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void ErrorInterpolated_Collection()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = new int[] {1, 2, 3};

        
        Logger.ErrorInterpolated($"{value}");
        Logger.Error("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void ErrorInterpolated_String()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        string value = "Test";

        
        Logger.ErrorInterpolated($"{value}");
        Logger.Error("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void ErrorInterpolated_Mixed()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.ErrorInterpolated(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Error(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void ErrorInterpolated_NamedScalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = 42;

        
        Logger.ErrorInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Error(            "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void ErrorInterpolated_NamedComplex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = new TestObject(3, 5);

        
        Logger.ErrorInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Error(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void ErrorInterpolated_LiteralValue([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "Test";

        
        Logger.ErrorInterpolated($"{new LiteralValue("Test", cacheable)}");
        Logger.Error(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void ErrorInterpolated_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "T{{es}}t";

        
        Logger.ErrorInterpolated($"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Error(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void ErrorInterpolated_MixedLiteralValue([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        
        Logger.ErrorInterpolated($"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void ErrorInterpolated_Literal_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"Test");
        Logger.Error(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void ErrorInterpolated_LiteralWithBraces_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"T{{es}}t");
        Logger.Error(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void ErrorInterpolated_Scalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value}");
        Logger.Error(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void ErrorInterpolated_ScalarWithAlignment_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value,3}");
        Logger.Error(exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void ErrorInterpolated_ScalarWithFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value:N}");
        Logger.Error(exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void ErrorInterpolated_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value,6:N}");
        Logger.Error(exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void ErrorInterpolated_Complex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value}");
        Logger.Error(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void ErrorInterpolated_Collection_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value}");
        Logger.Error(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void ErrorInterpolated_String_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{value}");
        Logger.Error(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void ErrorInterpolated_Mixed_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Error(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void ErrorInterpolated_NamedScalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Error(exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void ErrorInterpolated_NamedComplex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Error(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void ErrorInterpolated_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Error(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void ErrorInterpolated_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Error(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void ErrorInterpolated_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Error;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.ErrorInterpolated(exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Error);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion

    #region Fatal

    [Test]
    public void FatalInterpolated_Literal()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "Test";

        
        Logger.FatalInterpolated($"Test");
        Logger.Fatal(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void FatalInterpolated_LiteralWithBraces()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "T{{es}}t";

        
        Logger.FatalInterpolated($"T{{es}}t");
        Logger.Fatal(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void FatalInterpolated_Scalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        
        Logger.FatalInterpolated($"{value}");
        Logger.Fatal("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void FatalInterpolated_ScalarWithAlignment()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        
        Logger.FatalInterpolated($"{value,3}");
        Logger.Fatal("{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void FatalInterpolated_ScalarWithFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        
        Logger.FatalInterpolated($"{value:N}");
        Logger.Fatal("{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void FatalInterpolated_ScalarWithAlignmentAndFormat()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        
        Logger.FatalInterpolated($"{value,6:N}");
        Logger.Fatal("{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void FatalInterpolated_Complex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = new TestObject(3, 5);

        
        Logger.FatalInterpolated($"{value}");
        Logger.Fatal("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void FatalInterpolated_Collection()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = new int[] {1, 2, 3};

        
        Logger.FatalInterpolated($"{value}");
        Logger.Fatal("{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void FatalInterpolated_String()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        string value = "Test";

        
        Logger.FatalInterpolated($"{value}");
        Logger.Fatal("{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void FatalInterpolated_Mixed()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.FatalInterpolated(            $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Fatal(            "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void FatalInterpolated_NamedScalar()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = 42;

        
        Logger.FatalInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Fatal(            "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void FatalInterpolated_NamedComplex()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = new TestObject(3, 5);

        
        Logger.FatalInterpolated(            $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Fatal(            "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void FatalInterpolated_LiteralValue([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "Test";

        
        Logger.FatalInterpolated($"{new LiteralValue("Test", cacheable)}");
        Logger.Fatal(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void FatalInterpolated_LiteralValueWithBraces([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "T{{es}}t";

        
        Logger.FatalInterpolated($"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Fatal(message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void FatalInterpolated_MixedLiteralValue([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        
        Logger.FatalInterpolated($"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void FatalInterpolated_Literal_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"Test");
        Logger.Fatal(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void FatalInterpolated_LiteralWithBraces_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"T{{es}}t");
        Logger.Fatal(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void FatalInterpolated_Scalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value}");
        Logger.Fatal(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void FatalInterpolated_ScalarWithAlignment_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value,3}");
        Logger.Fatal(exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void FatalInterpolated_ScalarWithFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value:N}");
        Logger.Fatal(exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void FatalInterpolated_ScalarWithAlignmentAndFormat_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value,6:N}");
        Logger.Fatal(exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void FatalInterpolated_Complex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value}");
        Logger.Fatal(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void FatalInterpolated_Collection_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value}");
        Logger.Fatal(exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void FatalInterpolated_String_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{value}");
        Logger.Fatal(exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void FatalInterpolated_Mixed_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Fatal(exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void FatalInterpolated_NamedScalar_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Fatal(exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void FatalInterpolated_NamedComplex_WithException()
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Fatal(exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void FatalInterpolated_LiteralValue_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Fatal(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void FatalInterpolated_LiteralValueWithBraces_WithException([Values] bool cacheable)
    {
 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Fatal(exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void FatalInterpolated_MixedLiteralValue_WithException([Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

 
        const LogEventLevel logEventLevel = LogEventLevel.Fatal;
        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.FatalInterpolated(exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(LogEventLevel.Fatal);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion

    #region Write

    [Test]
    public void WriteInterpolated_Literal([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        const string message = "Test";

        
        Logger.WriteInterpolated(logEventLevel, $"Test");
        Logger.Write(logEventLevel, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WriteInterpolated_LiteralWithBraces([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        const string message = "T{{es}}t";

        
        Logger.WriteInterpolated(logEventLevel, $"T{{es}}t");
        Logger.Write(logEventLevel, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WriteInterpolated_Scalar([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        
        Logger.WriteInterpolated(logEventLevel, $"{value}");
        Logger.Write(logEventLevel, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WriteInterpolated_ScalarWithAlignment([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        
        Logger.WriteInterpolated(logEventLevel, $"{value,3}");
        Logger.Write(logEventLevel, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void WriteInterpolated_ScalarWithFormat([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        
        Logger.WriteInterpolated(logEventLevel, $"{value:N}");
        Logger.Write(logEventLevel, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void WriteInterpolated_ScalarWithAlignmentAndFormat([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        
        Logger.WriteInterpolated(logEventLevel, $"{value,6:N}");
        Logger.Write(logEventLevel, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void WriteInterpolated_Complex([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = new TestObject(3, 5);

        
        Logger.WriteInterpolated(logEventLevel, $"{value}");
        Logger.Write(logEventLevel, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void WriteInterpolated_Collection([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = new int[] {1, 2, 3};

        
        Logger.WriteInterpolated(logEventLevel, $"{value}");
        Logger.Write(logEventLevel, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WriteInterpolated_String([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        string value = "Test";

        
        Logger.WriteInterpolated(logEventLevel, $"{value}");
        Logger.Write(logEventLevel, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WriteInterpolated_Mixed([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        
        Logger.WriteInterpolated(logEventLevel,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Write(logEventLevel,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void WriteInterpolated_NamedScalar([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = 42;

        
        Logger.WriteInterpolated(logEventLevel,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Write(logEventLevel,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WriteInterpolated_NamedComplex([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = new TestObject(3, 5);

        
        Logger.WriteInterpolated(logEventLevel,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Write(logEventLevel,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WriteInterpolated_LiteralValue([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values] bool cacheable)
    {
        const string message = "Test";

        
        Logger.WriteInterpolated(logEventLevel, $"{new LiteralValue("Test", cacheable)}");
        Logger.Write(logEventLevel, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WriteInterpolated_LiteralValueWithBraces([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values] bool cacheable)
    {
        const string message = "T{{es}}t";

        
        Logger.WriteInterpolated(logEventLevel, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Write(logEventLevel, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WriteInterpolated_MixedLiteralValue([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

        
        Logger.WriteInterpolated(logEventLevel, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeNull();
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }

    [Test]
    public void WriteInterpolated_Literal_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"Test");
        Logger.Write(logEventLevel, exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WriteInterpolated_LiteralWithBraces_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"T{{es}}t");
        Logger.Write(logEventLevel, exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WriteInterpolated_Scalar_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value}");
        Logger.Write(logEventLevel, exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WriteInterpolated_ScalarWithAlignment_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value,3}");
        Logger.Write(logEventLevel, exception, "{value,3}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,3}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 3))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 3));
        }
    }

    [Test]
    public void WriteInterpolated_ScalarWithFormat_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value:N}");
        Logger.Write(logEventLevel, exception, "{value:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value:N}", format: "N", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, "N"));
        }
    }

    [Test]
    public void WriteInterpolated_ScalarWithAlignmentAndFormat_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        int value = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value,6:N}");
        Logger.Write(logEventLevel, exception, "{value,6:N}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value,6:N}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value,6:N}", format: "N", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 6))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value, 6, "N"));
        }
    }

    [Test]
    public void WriteInterpolated_Complex_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value}");
        Logger.Write(logEventLevel, exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));   
        }
    }

    [Test]
    public void WriteInterpolated_Collection_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = new int[] {1, 2, 3};

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value}");
        Logger.Write(logEventLevel, exception, "{@value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{@value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{@value}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WriteInterpolated_String_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        string value = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{value}");
        Logger.Write(logEventLevel, exception, "{value}", value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("{value}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[]
                {
                    new PropertyToken(nameof(value), "{value}", destructuring: Destructuring.Default, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(1);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be(Format(value));
        }
    }

    [Test]
    public void WriteInterpolated_Mixed_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] {5, 4, 3};
        var veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg = "veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongValue";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception,             $"ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{arg4}QRST{arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
        Logger.Write(logEventLevel, exception,             "ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ",
            arg1,
            arg2,
            arg3,
            arg4,
            arg5,
            veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("ABCD{arg1,7}EFGH{arg2:N}IJKL{arg3,-10:F3}MNOP{@arg4}QRST{@arg5}UVWX{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}YZ");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("ABCD"),
                    new PropertyToken(nameof(arg1), "{arg1,7}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Right, 7)),
                    new TextToken("EFGH"),
                    new PropertyToken(nameof(arg2), "{arg2:N}", destructuring: Destructuring.Default, alignment: default(Alignment), format: "N"),
                    new TextToken("IJKL"),
                    new PropertyToken(nameof(arg3), "{arg3,-10:F3}", destructuring: Destructuring.Default, alignment: new Alignment(AlignmentDirection.Left, 10), format: "F3"),
                    new TextToken("MNOP"),
                    new PropertyToken(nameof(arg4), "{@arg4}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("QRST"),
                    new PropertyToken(nameof(arg5), "{@arg5}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken("UVWX"),
                    new PropertyToken(nameof(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg), 
                        "{veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg}", 
                        destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("YZ")
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(6);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    arg1.ToLogEventPropertyValue(),
                    arg2.ToLogEventPropertyValue(),
                    arg3.ToLogEventPropertyValue(),
                    arg4.ToLogEventPropertyValue(),
                    arg5.ToLogEventPropertyValue(),
                    veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg.ToLogEventPropertyValue(),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"ABCD {Format(arg1, 7)}EFGH{Format(arg2, "N")}IJKL{Format(arg3,-10,"F3")}MNOP{Format(arg4)}QRST{Format(arg5)}UVWX{Format(veryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryVeryLongArg)}YZ");   
        }
    }

    [Test]
    public void WriteInterpolated_NamedScalar_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = 42;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Write(logEventLevel, exception,             "Default: {default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{default}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WriteInterpolated_NamedComplex_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel)
    {
        var value = new TestObject(3, 5);

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception,             $"Default: {value.Named("default")}, Stringified: {value.Named("$str")}, Destructured: {value.Named("@destructured")}");
        Logger.Write(logEventLevel, exception,             "Default: {@default}, Stringified: {$str}, Destructured: {@destructured}", value, value, value);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("Default: {@default}, Stringified: {$str}, Destructured: {@destructured}");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[]
                {
                    new TextToken("Default: "),
                    new PropertyToken("default", "{@default}", destructuring: Destructuring.Destructure, alignment: default(Alignment)),
                    new TextToken(", Stringified: "),
                    new PropertyToken("str", "{$str}", destructuring: Destructuring.Stringify, alignment: default(Alignment)),
                    new TextToken(", Destructured: "),
                    new PropertyToken("destructured", "{@destructured}", destructuring: Destructuring.Destructure, alignment: default(Alignment))
                },
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(3);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value.ToLogEventPropertyValue("default"),
                    value.ToString().ToLogEventPropertyValue("str"),
                    value.ToLogEventPropertyValue("destructured"),
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage().Should().Be($"Default: {Format(value)}, Stringified: {Format(value.ToString())}, Destructured: {Format(value)}");   
        }
    }

    [Test]
    public void WriteInterpolated_LiteralValue_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values] bool cacheable)
    {
        const string message = "Test";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{new LiteralValue("Test", cacheable)}");
        Logger.Write(logEventLevel, exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be(message);
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken(message) 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be(message);
        }
    }

    [Test]
    public void WriteInterpolated_LiteralValueWithBraces_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values] bool cacheable)
    {
        const string message = "T{{es}}t";

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"{new LiteralValue("T{es}t", cacheable)}");
        Logger.Write(logEventLevel, exception, message);

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(2);
            var logEvent = LogEvents.First();
            var serilogEvent = LogEvents.Last();
            logEvent.Should().BeEquivalentTo(serilogEvent, LogEventEquivalency);
            logEvent.MessageTemplate.Text.Should().Be("T{{es}}t");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new[] 
                { 
                    new TextToken("T{es}t") 
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(0);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("T{es}t");
        }
    }

    [Test]
    public void WriteInterpolated_MixedLiteralValue_WithException([ValueSource(nameof(LogEventLevels))] LogEventLevel logEventLevel, [Values] bool cacheable)
    {
        var value1 = 5;
        var value2 = 7;

        Exception exception = new InvalidOperationException("Test exception."); 
        Logger.WriteInterpolated(logEventLevel, exception, $"A{value1}B{new LiteralValue("C", cacheable)}D{value2}E");

        if (logEventLevel < MinLogEventLevel)
        {
            LogEvents.Should().BeEmpty();
        }
        else
        {
            LogEvents.Should().HaveCount(1);
            var logEvent = LogEvents.Single();
            logEvent.MessageTemplate.Text.Should().Be("A{value1}BCD{value2}E");
            logEvent.MessageTemplate.Tokens.Should().BeEquivalentTo(
                new MessageTemplateToken[] 
                { 
                    new TextToken("A"),
                    new PropertyToken(nameof(value1), "{value1}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("B"),
                    new TextToken("C"),
                    new TextToken("D"),
                    new PropertyToken(nameof(value2), "{value2}", destructuring: Destructuring.Default, alignment: default(Alignment)),
                    new TextToken("E")                    
                }, 
                MessageTemplateTokenEquivalency);
            logEvent.Properties.Count.Should().Be(2);
            logEvent.Properties.Should().BeEquivalentTo(
                new []
                {
                    value1.ToLogEventPropertyValue(),
                    value2.ToLogEventPropertyValue()
                },
                PropertiesEquivalency);
            logEvent.Level.Should().Be(logEventLevel);
            logEvent.Exception.Should().BeSameAs(exception);
            logEvent.RenderMessage(CultureInfo.InvariantCulture).Should().Be("A5BCD7E");
        }
    }


    #endregion
}