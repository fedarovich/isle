using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using FluentAssertions;
using Isle.Configuration;

namespace Isle.Core.Tests;

#if DEBUG
public class PrototypeLogHandlerTests
{
    [TearDown]
    public void TearDown()
    {
        IsleConfiguration.Reset();
    }

    [Test]
    public void WithDefaultValueRepresentationPolicy()
    {
        IsleConfiguration.Configure(_ => { });
            
        var var1 = 5d;
        var var2 = 3;
        var var3 = new int[] { 6, 7, 8 };
        var result = Log($"Abc{var1,6:F1}def{var2}gh{var3}");
        result.AsEnumerable().Should().BeEquivalentTo(
            new KeyValuePair<string, object>[]
            {
                new (nameof(var1), var1),
                new (nameof(var2), var2),
                new (nameof(var3), var3),
                new ("{OriginalFormat}", "Abc{var1,6:F1}def{var2}gh{var3}")
            });
        result.ToString().Should().BeEquivalentTo("Abc   5.0def3gh6, 7, 8");
            
        static FormattedLogValues Log([InterpolatedStringHandlerArgument] ref PrototypeLogHandler handler)
        {
            return handler.GetSegmentedLogValues();
        }
    }

    [Test]
    public void WithAutoDestructuringValueRepresentationPolicy()
    {
        IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring());

        var var1 = 5d;
        var var2 = 3;
        var var3 = new int[] { 6, 7, 8 };
        var result = Log($"Abc{var1,6:F1}def{var2}gh{var3}");
        result.AsEnumerable().Should().BeEquivalentTo(
            new KeyValuePair<string, object>[]
            {
                new (nameof(var1), var1),
                new (nameof(var2), var2),
                new (nameof(var3), var3),
                new ("{OriginalFormat}", "Abc{var1,6:F1}def{var2}gh{@var3}")
            });
        result.ToString().Should().BeEquivalentTo("Abc   5.0def3gh6, 7, 8");

        static FormattedLogValues Log([InterpolatedStringHandlerArgument] ref PrototypeLogHandler handler)
        {
            return handler.GetSegmentedLogValues();
        }
    }
}
#endif