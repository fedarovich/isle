using System.Collections.Generic;
using Serilog.Events;

namespace Isle.Serilog.Tests;

public class TestSinkOptions
{
    public IList<LogEvent> LogEvents { get; set; } = null!;
}