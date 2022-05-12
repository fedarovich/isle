using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging.Tests.MEL;

public readonly record struct TestLogItem(
    string Category,
    LogLevel LogLevel,
    EventId EventId,
    object? State,
    Exception? Exception,
    string Message,
    IReadOnlyCollection<object?> Scopes);