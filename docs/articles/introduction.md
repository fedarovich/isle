# Introduction

ISLE (Interpolated String Logging Extensions) lets you do structured logging in C# 10 or later with interpolated strings.

## The problem

Structured logging libraries such as `Microsoft.Extensions.Logging` and [Serilog](https://serilog.net) take a *message template* and a list of argument values:

```csharp
logger.LogInformation("Order {orderId} shipped to {city}", orderId, city);
```

The template names (`orderId`, `city`) become property names in the log event, so you can query and filter on them. But every name appears twice: once in the template and once as an argument. The compiler can't check that the two lists match. Arguments in the wrong order, or a renamed variable, quietly produce wrong logs.

Using an interpolated string instead of a template looks natural:

```csharp
logger.LogInformation($"Order {orderId} shipped to {city}"); // without ISLE: no structure!
```

Without ISLE, this formats the string first and logs a plain text message. The structure is lost, and the string is built even when the log level is disabled.

## The ISLE solution

ISLE adds logging methods that take [interpolated string handlers](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/interpolated-string-handler) instead of strings. For every hole in the interpolated string, the handler receives:

- the value;
- the alignment and format, if any;
- the **source text of the expression**, captured with [`CallerArgumentExpressionAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute).

ISLE uses the expression text as the property name and builds the message template from it. Both log calls below produce the template `Order {orderId} shipped to {city}`:

```csharp
logger.LogInformation("Order {orderId} shipped to {city}", orderId, city);
logger.LogInformation($"Order {orderId} shipped to {city}"); // with ISLE
```

## Benefits

- **No duplication.** Each name appears only once, so the names can't get out of sync with the values.
- **Fast.** ISLE builds the template from the interpolated string and never parses a template string. Templates are [cached](configuration.md#template-caching) by default.
- **Zero cost when disabled.** When a log level is disabled, the handler tells the compiler to skip the holes, so the argument expressions are not evaluated. To get the same with the standard methods, you have to wrap every call in `if (logger.IsEnabled(...))`.
- **Full control when you need it.** You can [rename arguments](argument-names.md), ask for [destructuring](value-representation.md), or put [literal text](literal-values.md) into the template.

## Supported logging libraries

| Logging library | Target interface | ISLE package | Methods |
| --------------- | ---------------- | ------------ | ------- |
| Microsoft.Extensions.Logging | `Microsoft.Extensions.Logging.ILogger` | `Isle.Extensions.Logging` | `Log`, `LogTrace`, `LogDebug`, `LogInformation`, `LogWarning`, `LogError`, `LogCritical`, `BeginScopeInterpolated` |
| Serilog | `Serilog.ILogger` | `Isle.Serilog` | `WriteInterpolated`, `VerboseInterpolated`, `DebugInterpolated`, `InformationInterpolated`, `WarningInterpolated`, `ErrorInterpolated`, `FatalInterpolated` |

> [!TIP]
> If your application uses Serilog only through the `Microsoft.Extensions.Logging.ILogger` facade, for example with `Serilog.Extensions.Hosting`, you only need `Isle.Extensions.Logging`.

## Packages

| Package | Description |
| ------- | ----------- |
| `Isle.Core` | Shared logic: configuration, value representation, name converters, `LiteralValue` and `Named()`. Referenced by every other package. |
| `Isle.Extensions.Logging` | Extension methods for `Microsoft.Extensions.Logging.ILogger`. |
| `Isle.Serilog` | Extension methods for `Serilog.ILogger`. |
| `Isle.Converters.Roslyn` | A [name converter](roslyn-name-converter.md) that uses the C# parser to extract clean names from expressions. |
| `Isle.Converters.Roslyn.Analyzers` | [Analyzers](code-analysis.md) that find captured names that can't be converted or that are duplicated. |
| `Isle.Compatibility` | Polyfills for `netstandard2.0`. You don't reference it directly. |

## Supported platforms

The ISLE libraries target `net11.0`, `net10.0`, `net9.0`, `net8.0` and `netstandard2.0`. Through .NET Standard 2.0, ISLE also works on .NET Framework 4.6.2 and later.

- **C# 10 or later is required**, because ISLE relies on interpolated string handlers and `CallerArgumentExpression`. If the language version is too old, the analyzer in `Isle.Core` reports error [ISLE0001](code-analysis.md#isle0001). On .NET Framework, set `<LangVersion>latest</LangVersion>` in your project.
- On .NET 8 and later, the libraries are marked as AOT-compatible (`IsAotCompatible`).
- Minimum dependency versions: `Microsoft.Extensions.Logging.Abstractions` 6.0.0 (for `netstandard2.0`; matching major versions on .NET 8+), and `Serilog` 3.0.0.

## Next steps

Continue with [Getting Started](getting-started.md).
