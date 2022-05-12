using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Isle.Extensions.Logging.Tests.MEL;

public class TestLogger : ILogger
{
    private readonly string _categoryName;
    private readonly IOptionsMonitor<TestLoggerOptions> _options;
    private readonly Stack<object?> _scopes = new ();
    private readonly IList<TestLogItem> _logItems;

    public TestLogger(string categoryName, IOptionsMonitor<TestLoggerOptions> options)
    {
        _categoryName = categoryName;
        _options = options;
        _logItems = options.CurrentValue.LogItems ?? new List<TestLogItem>();
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        _logItems.Add(new TestLogItem(_categoryName, logLevel, eventId, state, exception, formatter(state, exception), _scopes.ToArray()));
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.CurrentValue.MinLogLevel;

    public IDisposable BeginScope<TState>(TState state)
    {
        return new Scope(_scopes, state);
    }

    private class Scope : IDisposable
    {
        private Stack<object?>? _stack;
        private object? _state;

        public Scope(Stack<object?> stack, object? state)
        {
            _stack = stack;
            _state = state;
            stack.Push(state);
        }

        public void Dispose()
        {
            if (_stack == null)
                return;

            if (_stack.Peek() != _state)
                throw new InvalidOperationException();

            _stack.Pop();
            _stack = null;
            _state = null;
        }
    }
}