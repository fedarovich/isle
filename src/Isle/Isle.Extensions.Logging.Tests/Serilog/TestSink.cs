using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Extensions.Logging.Tests.Serilog
{
    public class TestSink : ILogEventSink
    {
        private readonly TestSinkOptions _options;

        public TestSink(TestSinkOptions options)
        {
            _options = options;
        }

        public void Emit(LogEvent logEvent)
        {
            _options.LogEvents.Add(logEvent);
        }
    }
}
