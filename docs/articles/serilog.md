# Serilog

The `Isle.Serilog` package adds interpolated-string logging methods to `Serilog.ILogger`. They are extension methods in the <xref:Isle.Serilog.LoggerExtensions> class (namespace `Isle.Serilog`, which the package adds as a global using).

> [!TIP]
> If you use Serilog only through the `Microsoft.Extensions.Logging.ILogger` facade, you don't need this package. Use [`Isle.Extensions.Logging`](microsoft-extensions-logging.md) instead.

## Setup

```bash
dotnet add package Isle.Serilog
```

```csharp
using Isle.Configuration;

IsleConfiguration.Configure(builder => builder.AddSerilog());
```

Serilog property names may contain only letters, digits and underscores. Captured expressions such as `order.Id` or `GetTotal()` aren't valid names, so we recommend a [name converter](argument-names.md#name-converters), for example the [Roslyn name converter](roslyn-name-converter.md):

```csharp
IsleConfiguration.Configure(builder => builder
    .AddSerilog()
    .WithRoslynNameConverter(options => options
        .CapitalizeFirstCharacter()
        .WithMemoization()));
```

## Logging methods

Each method has two overloads, with and without an `Exception`:

| Method | Log event level |
| ------ | --------------- |
| `VerboseInterpolated` | `LogEventLevel.Verbose` |
| `DebugInterpolated` | `LogEventLevel.Debug` |
| `InformationInterpolated` | `LogEventLevel.Information` |
| `WarningInterpolated` | `LogEventLevel.Warning` |
| `ErrorInterpolated` | `LogEventLevel.Error` |
| `FatalInterpolated` | `LogEventLevel.Fatal` |
| `WriteInterpolated` | passed as the `LogEventLevel logEventLevel` parameter |

```csharp
logger.InformationInterpolated($"User {userId} signed in");
logger.ErrorInterpolated(exception, $"Payment {paymentId} failed");
logger.WriteInterpolated(LogEventLevel.Warning, $"Disk {drive} is {percentFull}% full");
```

### Why the `Interpolated` suffix?

`Serilog.ILogger` declares `Information(string messageTemplate)` and the related methods as **instance** methods. C# always prefers an applicable instance method over an extension method, so an ISLE extension method called `Information` would never be chosen. That's why the ISLE methods have different names.

## How the event is written

When the level is enabled (`logger.IsEnabled(level)`), ISLE builds a `Serilog.Events.LogEvent` and passes it to `ILogger.Write(LogEvent)`:

- Property values are bound with `ILogger.BindProperty`, so your destructuring policies, `Destructure.ByTransforming<T>()` and similar settings still apply.
- Enrichers, filters, `ForContext(...)` properties and sinks work the same as with the standard methods.
- When the level is disabled, the argument expressions are not evaluated.

Because ISLE builds the template from the interpolated string, Serilog doesn't parse a template string. With [template caching](configuration.md#template-caching) (on by default), the parsed `MessageTemplate` objects are cached and reused.

## Alignment and format

Alignment and format specifiers are written into the message template:

```csharp
logger.InformationInterpolated($"Total: {total,10:N2}");
// Template: "Total: {total,10:N2}"
```

Serilog renders them as usual: a positive alignment right-aligns the value and a negative one left-aligns it.

## Destructuring

Prefix a C# identifier with `@` to destructure the value. In C#, `@point` is just another way to write `point`:

```csharp
logger.InformationInterpolated($"Moved to {@point}");
// Template: "Moved to {@point}"
```

See [Value Representation](value-representation.md) for more options, including automatic destructuring.

## Configuration options

`AddSerilog` takes an optional delegate that receives an <xref:Isle.Serilog.Configuration.ISerilogConfigurationBuilder>:

| Property | Default | Description |
| -------- | ------- | ----------- |
| `EnableMessageTemplateCaching` | `true` | Caches message templates. See [Template Caching](configuration.md#template-caching). |

```csharp
IsleConfiguration.Configure(builder => builder
    .AddSerilog(options => options.EnableMessageTemplateCaching = false));
```
