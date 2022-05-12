using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging.Tests.MEL;

public class TestLoggerOptions
{
    public LogLevel MinLogLevel { get; set; }

    public IList<TestLogItem>? LogItems { get; set; }
}