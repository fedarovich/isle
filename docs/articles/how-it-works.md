# How It Works

This article describes what ISLE does internally. You don't need it to use ISLE, but it explains the performance characteristics and why some of the rules exist.

## Interpolated string handlers

C# 10 introduced [interpolated string handlers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/interpolated-string-handler). When a method parameter's type is marked with `[InterpolatedStringHandler]`, the compiler doesn't build a string from an interpolated string argument. It creates the handler and passes each part to it separately.

Each ISLE logging method takes such a handler, one per log level, for example <xref:Isle.Extensions.Logging.InformationLogInterpolatedStringHandler>. A call like

```csharp
logger.LogInformation($"Area is {area}");
```

is compiled into code roughly like this:

```csharp
var handler = new InformationLogInterpolatedStringHandler(
    literalLength: 8, formattedCount: 1, logger, out bool isEnabled);

if (isEnabled)
{
    handler.AppendLiteral("Area is ");
    handler.AppendFormatted(area, alignment: 0, format: null, nameExpression: "area");
}

LoggerExtensions.LogInformation(logger, ref handler);
```

Three things follow from this:

1. **Disabled levels cost almost nothing.** The handler constructor calls `logger.IsEnabled(level)` and returns the result through `out bool isEnabled`. When the level is disabled, the compiler skips every `Append...` call, so the expressions in the holes are never evaluated.
2. **Names are known at compile time.** `AppendFormatted` has a `[CallerArgumentExpression("value")] string nameExpression` parameter, so the compiler passes the source text of each hole as a string constant.
3. **The hole value keeps its static type.** `AppendFormatted<T>` is generic, so the [value representation policy](value-representation.md) can decide based on `T`.

`LiteralValue` and `NamedLogValue` (the result of `.Named(...)`) have their own `AppendFormatted` overloads, which put text into the template and set an explicit name, respectively.

## From parts to a log event

The handler passes the parts to a *builder*. The builder's job is to produce the message template and collect the property values:

1. The captured name goes through the [name converter](argument-names.md#name-converters), if one is configured.
2. The [value representation policy](value-representation.md#value-representation-policies) may add a `@` or `$` prefix.
3. The literal parts are escaped (`{` → `{{`) and added to the template.

The result depends on the integration:

- **Microsoft.Extensions.Logging:** a state object that implements `IReadOnlyList<KeyValuePair<string, object?>>`, with one entry per hole and a final `{OriginalFormat}` entry. It is passed to `ILogger.Log`. On .NET (not .NET Standard), states with up to 7 properties use fixed-size specialized classes to avoid extra allocations.
- **Serilog:** a `LogEvent` with a `MessageTemplate` whose tokens are built directly, so no template string is parsed. It is passed to `ILogger.Write(LogEvent)`.

Builders are pooled per thread, so a log call usually doesn't allocate a new builder.

## Template caching

When [template caching](configuration.md#template-caching) is enabled (the default), the builder doesn't build the template string. It walks a process-wide **trie** instead:

- the root node is shared by all templates;
- each literal part and each hole (name, alignment and format) is a step down the trie;
- the node at the end of the walk holds the finished template string and the parsed segments or tokens.

The first call with a particular sequence of parts creates the nodes. Later calls only look them up and reuse the cached template. For Serilog, they reuse the parsed `MessageTemplate` object too.

The cache is safe for concurrent use and has no locks. A node first stores a single child in a field. When it gets a second child, it switches to a `ConcurrentDictionary`, using `Interlocked.CompareExchange`. Most nodes in real code have one child, so they stay small and fast.

Nodes are never removed, which is why names and cacheable [literal values](literal-values.md) must not be dynamic. A non-cacheable `LiteralValue` creates a separate node that isn't stored in the trie, so templates that contain it are not cached.

## Configuration as JIT-time constants

ISLE's settings are read on every log call, so reading them must be very cheap. Each layer (core, Microsoft.Extensions.Logging, Serilog) has an internal static class that copies the configuration into `static readonly` fields in its static constructor.

The JIT treats `static readonly` fields of a class whose static constructor has already run as **constants**. That means:

- branches like `if (EnableMessageTemplateCaching)` are resolved at JIT time, and the unused code path is removed;
- calls to the builders and the built-in value representation policies are devirtualized and can be inlined;
- the default policy check is removed completely.

This works only if the configuration can never change, which is why the configuration is [non-resettable](configuration.md#resettable-vs-non-resettable-configuration) by default. A resettable configuration makes every access go through a `volatile` snapshot. That is a bit slower, but it allows `IsleConfiguration.Reset()`.

The static constructors run once per process, the first time ISLE reads its settings. If no non-resettable configuration exists yet at that point, the process uses the resettable path for its whole lifetime. That's why you should configure ISLE [as early as possible](getting-started.md#where-to-put-the-configuration).

## Thread safety

- `IsleConfiguration.Configure` uses `Interlocked` operations and can safely be called while other threads log. Only the first call succeeds.
- The template cache has no locks and is safe for concurrent use.
- Builders are kept per thread.
- Memoizing [name converters](argument-names.md#memoization) use `ConcurrentDictionary`.
- Your own name converters and value representation policies are called concurrently, so they must be thread-safe.

## Benchmarks

The repository has [BenchmarkDotNet](https://benchmarkdotnet.org) projects that compare ISLE with the standard logging methods:

```bash
dotnet run -c Release -f net10.0 --project src/Isle/Isle.Extensions.Logging.Benchmarks -- --filter *
dotnet run -c Release -f net10.0 --project src/Isle/Isle.Serilog.Benchmarks -- --filter *
```
