using System.Collections.Generic;
using Serilog.Events;

namespace Isle.Extensions.Logging.Tests.Serilog
{
    public class TestSinkOptions
    {
        public IList<LogEvent> LogEvents { get; set; } = null!;
    }
}
