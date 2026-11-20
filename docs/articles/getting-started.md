# Getting Started

## 1. Install the package

Install the integration package for your logging library:

```bash
# For Microsoft.Extensions.Logging
dotnet add package Isle.Extensions.Logging

# For Serilog (Serilog.ILogger)
dotnet add package Isle.Serilog
```

You can install both. `Isle.Core` is installed automatically.

> [!TIP]
> If you use Serilog only through `Microsoft.Extensions.Logging.ILogger`, for example with `Serilog.AspNetCore` or `Serilog.Extensions.Hosting`, install only `Isle.Extensions.Logging`.

You still need a logging provider that understands structured logs: Serilog, the `Microsoft.Extensions.Logging` console logger with a JSON formatter, OpenTelemetry, and so on.

## 2. Configure ISLE

Configure ISLE **once**, at application startup, before you call any ISLE logging method. Add each integration you use:

```csharp
using Isle.Configuration;

// For Microsoft.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());

// For Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog());

// For both, in a single call
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .AddSerilog());
```

> [!IMPORTANT]
> - `IsleConfiguration.Configure` can be called **only once** per process. A second call throws `InvalidOperationException`. The only exception is a [resettable configuration](configuration.md#resettable-vs-non-resettable-configuration).
> - You **must** add every integration you use. An ISLE logging method called on an enabled log level throws `InvalidOperationException` if its integration was not added.

See [Configuration](configuration.md) for all options.

### Where to put the configuration

Call `Configure` as early as possible: at the top of `Program.cs` or `Main`, before the host is built.

```csharp
using Isle.Configuration;

IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());

var builder = WebApplication.CreateBuilder(args);
// ...
var app = builder.Build();
app.Run();
```

A [module initializer](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/module-initializers) in your **executable** project runs before any other code in the assembly, so it is a good place for the configuration:

```csharp
using System.Runtime.CompilerServices;
using Isle.Configuration;

internal static class IsleSetup
{
    [ModuleInitializer]
    internal static void Initialize() =>
        IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
}
```

ISLE reads its configuration into `static readonly` fields, which the JIT treats as constants. Configuring early lets ISLE use this faster path for the whole process. [How It Works](how-it-works.md#configuration-as-jit-time-constants) explains the details.

> [!NOTE]
> Configure ISLE in **applications**, not in **libraries**. The configuration is process-wide and can be set only once, so a library that calls `Configure` would clash with the application. A library can use the ISLE logging methods freely; the application that hosts it configures ISLE.

## 3. Log with interpolated strings

With `Microsoft.Extensions.Logging`:

```csharp
int width = 5;
int height = 6;
int area = width * height;

logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {area}");
```

With Serilog, the methods have the `Interpolated` suffix:

```csharp
logger.InformationInterpolated($"The area of rectangle with the width = {width} and the height = {height} is {area}");
```

Both produce the message template `The area of rectangle with the width = {width} and the height = {height} is {area}`, with the properties `width`, `height` and `area`. It's the same log event as the classic call:

```csharp
logger.LogInformation("The area of rectangle with the width = {width} and the height = {height} is {area}", width, height, area);
```

> [!NOTE]
> The `Isle.Extensions.Logging` methods have the same names as the standard `Microsoft.Extensions.Logging` extension methods (`LogInformation`, `LogError`, ...). For an interpolated string argument, the C# compiler picks the ISLE overload.
>
> The `Isle.Serilog` methods have the `Interpolated` suffix (`InformationInterpolated`, ...). `Serilog.ILogger` declares `Information(...)` and the other methods as *instance* methods, and C# always prefers an instance method over an extension method.

## Namespaces

The packages add these `global using` directives to your project automatically:

| Package | Global using | Gives you |
| ------- | ------------ | --------- |
| `Isle.Core` | `Isle.Extensions` | the `.Named(...)` extension method |
| `Isle.Extensions.Logging` | `Isle.Extensions.Logging` | the `ILogger` extension methods |
| `Isle.Serilog` | `Isle.Serilog` | the Serilog `ILogger` extension methods |

You can turn each one off with an [MSBuild property](msbuild-properties.md#global-usings). Other namespaces need an explicit `using`:

- `Isle.Configuration`: `IsleConfiguration`, the configuration builder and its extension methods;
- `Isle.Converters`: `ValueNameConverters`;
- `Isle`: `LiteralValue`.

> [!NOTE]
> Global usings need an SDK-style project. In an old-style project that uses `packages.config`, add the `using` directives yourself.

## Troubleshooting

### The message is logged as plain text without properties

The C# compiler chose the standard method that takes a `string`, not the ISLE overload. ISLE overloads are used only when **an interpolated string is passed directly** as the argument. These calls don't use ISLE:

```csharp
string message = $"Order {orderId} shipped";
logger.LogInformation(message);                     // a string, not an interpolated string

logger.LogInformation("Order " + orderId);          // not an interpolated string
```

Also check where the `using Microsoft.Extensions.Logging;` directive is. C# looks up extension methods scope by scope, starting from the innermost one. If that directive is *inside* a namespace declaration, the standard methods are found first and the ISLE methods, which come from a `global using`, are never considered. Move the directive to the top of the file, outside the namespace, or add `using Isle.Extensions.Logging;` next to it.

### `InvalidOperationException`: "... is not configured"

The messages are "Isle is not configured", "Isle.Extension.Logging is not configured" and "Isle.Serilog is not configured". An ISLE logging method was called on an enabled log level, and one of these is true:

- `IsleConfiguration.Configure` has not been called yet. Move the configuration earlier; a module initializer is the safest place.
- `Configure` was called, but the integration was not added. Add `builder.AddExtensionsLogging()` or `builder.AddSerilog()` to the configuration.

## Next steps

- [Microsoft.Extensions.Logging](microsoft-extensions-logging.md) and [Serilog](serilog.md): all logging methods.
- [Argument Names](argument-names.md): control the property names.
- [Value Representation](value-representation.md): destructuring and stringification.
