using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Isle.Serilog.Tests;

public static class TestSinkExtensions
{
    public static LoggerConfiguration TestSink(
        this LoggerSinkConfiguration loggerConfiguration,
        Action<TestSinkOptions> configure)
    {
        var options = new TestSinkOptions();
        configure.Invoke(options);
        options.LogEvents ??= new List<LogEvent>();

        return loggerConfiguration.Sink(new TestSink(options));
    }
}