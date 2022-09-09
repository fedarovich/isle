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
public class LoggerExtensions2Tests: BaseFixture {
    public LoggerExtensions2Tests(LogEventLevel minLogEventLevel, bool enableCaching) : base(minLogEventLevel, enableCaching) {
    }

    [Test]
    public void VerboseInterpolated2_Mixed() {

        var arg1 = "Test";
        var arg2 = 5000;
        var arg3 = 4.5;
        var arg4 = new TestObject(7, 11);
        var arg5 = new int[] { 5, 4, 3 };

        for(int i = 0; i < 2; ++i) {
            Logger.VerboseInterpolated2($"ABCD{arg1}EFGH{arg2}IJKL{arg3}MNOP{arg4}QRST{arg5}UVWX");
        }

        LogEvents.Should().HaveCount(2);
    }
}
