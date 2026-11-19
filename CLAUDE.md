# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

ISLE (Interpolated String Logging Extensions) is a set of NuGet libraries that let C# 10+ code do structured logging with interpolated strings (`logger.LogInformation($"Area is {area}")`). The argument names are captured with `[CallerArgumentExpression]` and turned into a message template. The libraries target `net11.0;net10.0;net9.0;net8.0;netstandard2.0`. User-facing docs are in `README.md`.

## Commands

The solution is `src/Isle/Isle.sln`. Run commands from the repo root:

```powershell
dotnet build src/Isle/Isle.sln -c Release
dotnet test  src/Isle/Isle.sln -c Release

# Fast loop: one test project, one TFM
dotnet test src/Isle/Isle.Core.Tests -f net10.0

# Single test / fixture (NUnit). Fixtures use [TestFixtureSource], so match on Name or FullyQualifiedName with ~
dotnet test src/Isle/Isle.Extensions.Logging.Tests -f net10.0 --filter "Name~LogInformation_Literal"
dotnet test src/Isle/Isle.Serilog.Tests -f net10.0 --filter "FullyQualifiedName~Isle.Serilog.Tests.Caching"

# Benchmarks (BenchmarkDotNet, BenchmarkSwitcher)
dotnet run -c Release -f net10.0 --project src/Isle/Isle.Extensions.Logging.Benchmarks -- --filter *
```

- Test projects target `net11.0;net10.0;net9.0;net8.0;net48`. Running them without `-f` requires every one of those runtimes; CI installs the .NET 8, 9, 10 and 11 SDKs for this. `Isle.Converters.Roslyn.Analyzers.Test` targets `net10.0` only.
- Tests use NUnit 3 and FluentAssertions 7 (NSubstitute in `Isle.Core.Tests`).
- CI is GitHub Actions (`.github/workflows/build.yml`, `windows-2025`). It builds, tests and packs 6 packages: Compatibility, Core, Extensions.Logging, Serilog, Converters.Roslyn, and Converters.Roslyn.Analyzers (packed from the `.Package` project).
  - Push to `dev`: CI build, published to MyGet as `<VERSION_PREFIX>-pre.<run_number>`.
  - Pull requests to `dev`/`main`: build, test and pack only.
  - Manual run (`workflow_dispatch`, `build-type` = `Pre-release`/`Release`): published to nuget.org via trusted publishing (`NuGet/login`, `nuget` environment).
  - The version comes from `VERSION_PREFIX` in the workflow plus `github.run_number`; bump `VERSION_PREFIX` after each release.

## T4 templates: generated code is checked in

Much of the public surface and many tests are generated from T4 templates (`*.tt` → `*.g.cs`, using `TextTemplatingFileGenerator`). The build does **not** run T4. Change the `.tt` file, regenerate the `.g.cs` (in Visual Studio: "Run Custom Tool"), and commit both files. Never hand-edit `*.g.cs`.

- `Isle.Extensions.Logging/LogHandlers.tt`: one `[InterpolatedStringHandler] ref struct` per log level, plus a generic `Log(LogLevel)` handler and a `BeginScope` handler.
- `Isle.Extensions.Logging/LoggerExtensions.tt`: the `ILogger` extension methods that take those handlers.
- `Isle.Extensions.Logging/FormattedLogValues.tt`: fixed-size `FormattedLogValues0..7` state objects (only for `NETCOREAPP`).
- `Isle.Serilog/LogHandlers.tt` and `LoggerExtensions.tt`: the same pattern for `Serilog.ILogger`. These methods use an `*Interpolated` suffix because of overload resolution.
- Test projects: `LogHandlerTests.tt`, `LoggerExtensionsTests.tt`, `MELTests.tt`, `SerilogLoggerExtensionsTests.tt`.

## Architecture

### Project layering

`Isle.Compatibility` (polyfills such as `DefaultInterpolatedStringHandler`, `Index`/`Range`, and attributes; referenced only for the `netstandard2.0` TFM) → `Isle.Core` → `Isle.Extensions.Logging` / `Isle.Serilog` / `Isle.Converters.Roslyn`. Core shares internals with the integration projects and tests through `InternalsVisibleTo`, so internal types such as `CoreConfiguration` and `ValueStringBuilder` are effectively shared API.

`Isle.Core.Analyzers` (`ISLE0001`, which requires C# 10) is not referenced as an assembly. `Isle.Core.csproj` packs its DLL into `analyzers/dotnet/cs`. The `buildTransitive/*.targets` files add global usings (`Isle.Extensions`, `Isle.Serilog`). Consumers can opt out with `UsingIsleExtensions=False` and `UsingIsleSerilog=False`.

### Configuration: JIT-constant pattern (performance-critical)

Configuration has three layers, and each integration repeats the same layers:

1. **`IsleConfiguration`** (public): `Configure(...)` can succeed only once. It builds a `CoreConfigurationSnapshot` and then calls every registered `IIsleExtensionConfigurationHook`. `AddExtensionsLogging()` and `AddSerilog()` register these hooks, and each hook sets its own `*ConfigurationSnapshot`.
2. **`*ConfigurationSnapshot`** (internal): an immutable holder in a `volatile` static that can be set once.
3. **`CoreConfiguration`, `ExtensionsLoggingConfiguration`, `SerilogConfiguration`** (internal static): the static constructor copies the snapshot into `static readonly` fields **if the snapshot is non-resettable**. The JIT then treats those fields as constants, so it removes dead branches and devirtualizes calls. Every hot-path accessor checks `IsResettable` first and otherwise reads the snapshot.

What this means in practice:
- Hot-path code should read settings through the internal static `*Configuration` classes, not through `IsleConfiguration.Current`. Keep each accessor in the `IsResettable ? snapshot : readonly-field` shape.
- The static constructor runs once per process. Whichever configuration is in place when it first runs decides the mode for the whole process. Tests therefore always configure with `.IsResettable()` and call `IsleConfiguration.Reset()` in teardown (see `BaseFixture` in each test project). `Reset()` throws on a non-resettable configuration.
- Some type checks only work because of the constant folding, for example `CoreConfiguration.GetRepresentationOfType<T>` comparing against known policy types. Comments there say that some helper indirections exist only to make inlining work. Don't "simplify" them.

### Logging pipeline

Interpolated string handler (generated) → builder → logger:

- The handler constructor calls `logger.IsEnabled(level)` and receives an `out bool isEnabled`. When logging is disabled, the compiler skips every `Append*` call, so argument expressions are never evaluated.
- `AppendFormatted<T>(T value, ..., [CallerArgumentExpression] string nameExpression)` turns the expression into a name with `GetNameFromCallerArgumentExpression<T>()`. That method applies the name converter and the value-representation policy (`@` for destructure, `$` for stringify). `NamedLogValue` (`.Named("x")`) and `LiteralValue` have their own `AppendFormatted` overloads.
- There are two builder strategies, chosen by `EnableMessageTemplateCaching`:
  - **Simple** (`SimpleFormattedLogValuesBuilder` for MEL, `SimpleLogEventBuilder` for Serilog): builds the template string on every call.
  - **Caching** (`CachingFormattedLogValuesBuilder`, `CachingLogEventBuilder`): walks a process-wide trie rooted at `Caching/NodeCache.Instance`. Literal and hole nodes are keyed by literal text, hole name, or `FormatKey` (name, alignment, format). The terminal `TemplateNode` holds the cached template string and the parsed segments or tokens. Each node first stores a single child and upgrades lock-free (`Interlocked.CompareExchange`) to a `ConcurrentDictionary` when it gets a second child. A `LiteralValue` that is not cacheable creates a non-cached node, which is why caching non-constant literals leaks memory.
- Builders are pooled per thread (`[ThreadStatic]`). With non-resettable configuration, the handlers call `Unsafe.As<ConcreteBuilder>(_builder)` to devirtualize the call.
- On the MEL side, the logger state is a `FormattedLogValuesBase` (`IReadOnlyList<KeyValuePair<string, object?>>`, ending with `{OriginalFormat}`). The Serilog side builds a `LogEvent` directly and writes it to the logger. MEL and Serilog each have their own `Caching/` node types (`LiteralNode`/`HoleNode` vs `TextNode`/`PropertyNode`).

### Roslyn name converter and its analyzer

- `Isle.Converters.Roslyn/RoslynNameConverter` parses the captured argument expression and unwraps it (identifiers, member access, `?.`, invocations, casts, `++`/`--`, `*`, `!`, `checked`/`unchecked`, parentheses) down to a member name. It then applies an ordered list of transformations, such as `CapitalizeFirstCharacter` and `RemoveMethodPrefixes`. Configure it with `WithRoslynNameConverter(...)`.
- `Isle.Converters.Roslyn.Analyzers/CapturedNamesAnalyzer` (`ISLE4000`–`ISLE4004`, with code fixes in `.CodeFixes`) **reimplements the same unwrapping logic** in its local `TryGetName` and has its own private `NameExpressionType`. When the supported expressions change in `RoslynNameConverter.Unwrap`, update the analyzer to match. The analyzer gets the prefixes to remove from the MSBuild properties `IsleRoslynNameConverterRemoveMethodPrefixes` and `...StringComparison`, which the package's `buildTransitive` targets expose as `CompilerVisibleProperty`.
- The analyzer and code-fix projects target `netstandard2.0` with `IsPackable=false`. Only `Isle.Converters.Roslyn.Analyzers.Package` produces the NuGet package.

## Conventions

- `LangVersion` is `LatestMajor`, with `Nullable` and `AllowUnsafeBlocks` enabled. `IsAotCompatible` is set for `net7.0+` TFMs.
- Code that differs per TFM uses `#if NETCOREAPP`, `NETFRAMEWORK || NETSTANDARD2_0`, `NET6_0_OR_GREATER`, and so on. Anything new that must work on `netstandard2.0` may need a polyfill in `Isle.Compatibility`.
- `Microsoft.Extensions.Logging.Abstractions` is pinned per TFM (6.0 for `netstandard2.0`/`net48`, then 8/9/10/11 to match). Serilog is pinned to 3.0.0.
