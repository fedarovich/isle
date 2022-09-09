using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Isle.Serilog {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Implemented in TLogLevel.g.cs
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public interface IHasLogLevel<TSelf> where TSelf : IHasLogLevel<TSelf> {
        static abstract LogEventLevel GetLevel();
    }

    enum StringPart {Literal, Formatted }

    [InterpolatedStringHandler]
    public ref struct LogISH<TLogLevel> where TLogLevel : IHasLogLevel<TLogLevel> {
        //private readonly List<(StringPart StringPart,string Value)>? StringParts = default;

        private readonly (StringPart Type, string LiteralOrName, object? V)[]? StringParts = default;

        private readonly int LiteralLength;
        private readonly int FormattedCount;

        public bool IsEnabled => StringParts != null;

        int stringPartCount = 0;

        static ArrayPool<(StringPart Type, string LiteralOrName, object? V)> StringPartsArrayPool => ArrayPool<(StringPart Type, string LiteralOrName, object? V)>.Shared;

        public LogISH(
            int literalLength,
            int formattedCount,
            ILogger logger,
            out bool isEnabled
        ) {
            isEnabled = logger.IsEnabled(TLogLevel.GetLevel());
            LiteralLength = literalLength;
            FormattedCount = formattedCount;
            if (!isEnabled) return;

            // formattedCount*2 + 1 is a reasonable assumption of the max number of string parts, but the spec
            // offers no insights into whether AppendLiteral might be called multiple times consecutively, for
            // instance in the case of added interpolated string $"aa{1}bb" + $"cc{2}" we might see "bb" and "cc"
            // in seperate calls to AppendLiteral.
            // So it might be a good idea to a an overflow check to the Append* methods and possibly Rent a larger
            // array
            StringParts = StringPartsArrayPool.Rent(formattedCount*2 + 1);
        }

        void Reset() {
            StringPartsArrayPool.Return(StringParts!);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendLiteral(string literal) =>
            StringParts![stringPartCount++] = (StringPart.Literal, literal, null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendFormatted<T>(T value, [CallerArgumentExpression("value")] string name = "") {
            StringParts![stringPartCount++] = (StringPart.Formatted, name, value);
        }


        static readonly ConcurrentDictionary<string, MessageTemplate> CachedTemplates = new();
        internal LogEvent GetLogEvent(ILogger logger, string handlerExpr, Exception? exception = null) {
            if (CachedTemplates.TryGetValue(handlerExpr, out var template)) {
                var logEvent = new LogEvent(
                    DateTimeOffset.Now,
                    TLogLevel.GetLevel(),
                    exception,
                    template,
                    Array.Empty<LogEventProperty>());

                foreach (var sp in StringParts.AsSpan(0, stringPartCount)) {
                    if(sp.Type == StringPart.Formatted
                        &&logger.BindProperty(sp.LiteralOrName, sp.V, true, out var property)) 
                    {
                        logEvent.AddOrUpdateProperty(property);
                    }
                }
                Reset();
                return logEvent;
            }

            SimpleLogEventBuilder builder = new();
            builder.PublicInitialize(LiteralLength, FormattedCount, logger);
            foreach(var sp in StringParts.AsSpan(0, stringPartCount)) {
                switch (sp.Type) {
                    case StringPart.Literal:
                        builder.AppendLiteral(sp.LiteralOrName);
                        break;
                    case StringPart.Formatted:
                        builder.AppendFormatted(sp.LiteralOrName, sp.V, 0, null);
                        break;
                }
            }
            Reset();
            template = builder.GetMessageTemplate();
            CachedTemplates.TryAdd(handlerExpr, template);
            return builder.BuildAndReset(template, TLogLevel.GetLevel(), exception);
        }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}
