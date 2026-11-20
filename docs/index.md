---
_layout: landing
---

# ISLE

<div class="isle-hero">
  <img src="images/logo.png" alt="ISLE logo">
  <p><strong>ISLE</strong> (Interpolated String Logging Extensions) lets you do structured logging with C# interpolated strings.
  You write ordinary interpolated strings. ISLE captures the argument names and turns the string into a message template.</p>
</div>

```csharp
int width = 5;
int height = 6;
int area = width * height;

// Without ISLE: the names are repeated in the template
logger.LogInformation("The area of rectangle with the width = {width} and the height = {height} is {area}", width, height, area);

// With ISLE: the same structured log event
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {area}");
```

Both calls produce the same message template, `The area of rectangle with the width = {width} and the height = {height} is {area}`, with three properties: `width`, `height` and `area`.

## Why ISLE?

- **No duplication.** You don't repeat argument names in a template string, so names and values can't get out of sync.
- **Fast.** ISLE builds the message template directly from the interpolated string, so it never parses a template string. Templates are cached by default, so each distinct template is built only once. This can make ISLE faster than the standard logging methods.
- **Zero cost when disabled.** When a log level is disabled, the argument expressions are not evaluated and the underlying logger is not called. You don't need an `if (logger.IsEnabled(...))` guard.
- **Works everywhere.** ISLE supports .NET 8 and later, and .NET Standard 2.0 (.NET Framework 4.6.2 and later). It is trimming and Native AOT compatible.

## Packages

| Package | Description |
| ------- | ----------- |
| [Isle.Extensions.Logging](https://www.nuget.org/packages/Isle.Extensions.Logging) | Integration with `Microsoft.Extensions.Logging`. |
| [Isle.Serilog](https://www.nuget.org/packages/Isle.Serilog) | Integration with Serilog. |
| [Isle.Converters.Roslyn](https://www.nuget.org/packages/Isle.Converters.Roslyn) | Roslyn-based argument name converter. |
| [Isle.Converters.Roslyn.Analyzers](https://www.nuget.org/packages/Isle.Converters.Roslyn.Analyzers) | Code analyzers for the Roslyn-based name converter. |
| [Isle.Core](https://www.nuget.org/packages/Isle.Core) | Shared core logic. It is installed with the integration packages. |
| [Isle.Compatibility](https://www.nuget.org/packages/Isle.Compatibility) | Polyfills for `netstandard2.0`. It is installed automatically when needed. |

## Next steps

- [Getting Started](articles/getting-started.md): install ISLE and log your first message.
- [Configuration](articles/configuration.md): all configuration options.
- [API Reference](api/Isle.Configuration.yml): the public API, generated from the source code.
- [GitHub repository](https://github.com/fedarovich/isle): source code, issues and releases.
