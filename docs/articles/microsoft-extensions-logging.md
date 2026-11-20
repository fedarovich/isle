# Microsoft.Extensions.Logging

The `Isle.Extensions.Logging` package adds interpolated-string overloads to `Microsoft.Extensions.Logging.ILogger`. They are extension methods in the <xref:Isle.Extensions.Logging.LoggerExtensions> class (namespace `Isle.Extensions.Logging`, which the package adds as a global using).

## Setup

```bash
dotnet add package Isle.Extensions.Logging
```

```csharp
using Isle.Configuration;

IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
```

## Logging methods

There is one method per log level, each with four overloads:

| Method | Log level |
| ------ | --------- |
| `LogTrace` | `LogLevel.Trace` |
| `LogDebug` | `LogLevel.Debug` |
| `LogInformation` | `LogLevel.Information` |
| `LogWarning` | `LogLevel.Warning` |
| `LogError` | `LogLevel.Error` |
| `LogCritical` | `LogLevel.Critical` |
| `Log` | passed as the `LogLevel logLevel` parameter |

The overloads take an optional `EventId` and an optional `Exception`, in the same order as the standard methods:

```csharp
logger.LogInformation($"User {userId} signed in");
logger.LogWarning(exception, $"Retrying request {requestId} after {delay}");
logger.LogError(new EventId(1001, "PaymentFailed"), $"Payment {paymentId} failed");
logger.LogCritical(new EventId(1002), exception, $"Lost connection to {host}");

// The level is chosen at runtime
logger.Log(level, $"Cache {cacheName} has {count} entries");
logger.Log(level, eventId, exception, $"Cache {cacheName} has {count} entries");
```

### Disabled log levels

Each method checks `logger.IsEnabled(level)` **before** any part of the interpolated string is evaluated. If the level is disabled, the argument expressions are not evaluated at all and `ILogger.Log` is not called:

```csharp
// GetExpensiveDiagnostics() runs only if Debug is enabled.
logger.LogDebug($"Diagnostics: {GetExpensiveDiagnostics()}");
```

So with ISLE you don't need the usual `if (logger.IsEnabled(LogLevel.Debug))` guard.

> [!WARNING]
> Don't put side effects into interpolation holes, for example `{counter++}`. Whether they run depends on the logging configuration.

## Scopes

`BeginScopeInterpolated` creates a logging scope with structured state:

```csharp
using (logger.BeginScopeInterpolated($"Processing order {orderId} for {customerId}"))
{
    logger.LogInformation($"Validating {itemCount} items");
}
```

The scope state is a list of key/value pairs (`orderId`, `customerId` and `{OriginalFormat}`), the same as the state of `ILogger.BeginScope("Processing order {orderId} for {customerId}", orderId, customerId)`.

The method has a different name from `BeginScope` because MEL's own `BeginScope<TState>(TState state)` is an instance method of `ILogger`, and C# always prefers an instance method over an extension method.

> [!NOTE]
> Scopes have no log level, so `BeginScopeInterpolated` always evaluates its arguments and always creates the scope state.

## Alignment and format

Alignment and format specifiers in the holes become part of the message template:

```csharp
logger.LogInformation($"Total: {total,10:N2}");
// Template: "Total: {total,10:N2}"
```

The formatted message (what `ToString()` of the state returns, which text-based providers write) uses these rules:

- it is formatted with `CultureInfo.InvariantCulture`;
- `null` values are written as `(null)`;
- collections (any `IEnumerable` other than `string`) are written as their items separated by `", "`, as in standard `Microsoft.Extensions.Logging`.

## Log state

The state passed to `ILogger.Log` implements `IReadOnlyList<KeyValuePair<string, object?>>`, the same shape the standard `LoggerExtensions` methods produce:

- one entry per hole, keyed by the captured name. The key includes the `@` or `$` operator, if any; see [Value Representation](value-representation.md);
- a final `{OriginalFormat}` entry with the message template.

So any provider that supports structured logging with the standard methods also works with ISLE.

## Using Serilog as the provider

If your application uses Serilog through `Microsoft.Extensions.Logging`, for example with `Serilog.AspNetCore` or `Serilog.Extensions.Hosting`, use `Isle.Extensions.Logging`. The Serilog provider understands the `@` (destructure) and `$` (stringify) operators in the state keys.

Serilog property names may contain only letters, digits and underscores, so add a name converter. The [Roslyn name converter](roslyn-name-converter.md) works best; `ValueNameConverters.SerilogCompatible()` is a simpler alternative:

```csharp
using Isle.Configuration;
using Isle.Converters;

IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithNameConverter(ValueNameConverters.SerilogCompatible().WithMemoization()));
```

## Configuration options

`AddExtensionsLogging` takes an optional delegate that receives an <xref:Isle.Extensions.Logging.Configuration.IExtensionsLoggingConfigurationBuilder>:

| Property | Default | Description |
| -------- | ------- | ----------- |
| `EnableMessageTemplateCaching` | `true` | Caches message templates. See [Template Caching](configuration.md#template-caching). |

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging(options => options.EnableMessageTemplateCaching = false));
```
