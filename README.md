# ISLE

![Isle logo](/IsleLogo.png)

ISLE (Interpolated String Logging Extensions) is a library that allows developers to perform structured logging using interpolated strings in C# 10 or later.

**📖 Documentation: [isle.fedarovich.com](https://isle.fedarovich.com/)**

| Package | Release | Prerelease | Dev Build | Description |
| ------- | ---------- | ------- | --------- | ----------- |
| Isle.Core | [![nuget](https://img.shields.io/nuget/v/Isle.Core.svg?label=nuget)](https://www.nuget.org/packages/Isle.Core) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Core.svg?label=nuget)](https://www.nuget.org/packages/Isle.Core) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Core.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Core) | Core package containing shared logic. |
| Isle.Extensions.Logging | [![nuget](https://img.shields.io/nuget/v/Isle.Extensions.Logging.svg?label=nuget)](https://www.nuget.org/packages/Isle.Extensions.Logging) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Extensions.Logging.svg?label=nuget)](https://www.nuget.org/packages/Isle.Extensions.Logging) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Extensions.Logging.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Extensions.Logging) | Integration package for Microsoft.Extensions.Logging. |
| Isle.Serilog | [![nuget](https://img.shields.io/nuget/v/Isle.Serilog.svg?label=nuget)](https://www.nuget.org/packages/Isle.Serilog) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Serilog.svg?label=nuget)](https://www.nuget.org/packages/Isle.Serilog) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Serilog.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Serilog) | Integration package for Serilog. |
| Isle.Converters.Roslyn | [![nuget](https://img.shields.io/nuget/v/Isle.Converters.Roslyn.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Converters.Roslyn.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Converters.Roslyn.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Converters.Roslyn) | Roslyn-based name converter. |
| Isle.Converters.Roslyn.Analyzers | [![nuget](https://img.shields.io/nuget/v/Isle.Converters.Roslyn.Analyzers.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn.Analyzers) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Converters.Roslyn.Analyzers.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn.Analyzers) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Converters.Roslyn.Analyzers.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Converters.Roslyn.Analyzers) | Code analyzers for Roslyn-based name converter. |
| Isle.Compatibility | [![nuget](https://img.shields.io/nuget/v/Isle.Compatibility.svg?label=nuget)](https://www.nuget.org/packages/Isle.Compatibility) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Compatibility.svg?label=nuget)](https://www.nuget.org/packages/Isle.Compatibility) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Compatibility.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Compatibility) | Compatibility package for `netstandard2.0`. |

[![Build](https://github.com/fedarovich/isle/actions/workflows/build.yml/badge.svg?branch=dev)](https://github.com/fedarovich/isle/actions/workflows/build.yml)
[![Docs](https://github.com/fedarovich/isle/actions/workflows/docs.yml/badge.svg?branch=main)](https://isle.fedarovich.com/)
[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/fedarovich/isle/blob/main/LICENSE)

ISLE works on .NET 8 and later, and also supports .NET Standard 2.0, so it can be used with .NET Framework 4.6.2 or later.

> [!IMPORTANT]
> ISLE v2.x introduces some breaking changes in its configuration. If you are upgrading from v1.x, see [Migrating from v1.x to v2.x](https://isle.fedarovich.com/articles/migration-v2.html).

## Why ISLE?

+ ISLE allows you to use C# interpolated strings in loggers instead of passing a message template and arguments separately.
+ ISLE is fast. It builds message templates directly from interpolated strings and caches them, so it can be faster than the standard logging methods.
+ ISLE logging methods are no-op for disabled log levels. If a log level is disabled, the argument values are not evaluated, and the underlying `Microsoft.Extensions.Logging.ILogger.Log` or `Serilog.ILogger.Write` method isn't called.

## Quick Start

Install the package for your logging library:

```bash
dotnet add package Isle.Extensions.Logging
# AND/OR
dotnet add package Isle.Serilog
```

Configure ISLE once, at the beginning of your program:

```cs
using Isle.Configuration;

// For use with Microsoft.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
// or for use with Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog());
```

Now pass interpolated strings to the logger. ISLE captures the variable names and builds the message template for structured logging:

```cs
int width = 5;
int height = 6;
int area = width * height;

// Microsoft.Extensions.Logging
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {area}");

// Serilog
logger.InformationInterpolated($"The area of rectangle with the width = {width} and the height = {height} is {area}");
```

This produces the same structured log event as:

```cs
logger.LogInformation("The area of rectangle with the width = {width} and the height = {height} is {area}", width, height, area);
```

## Documentation

The full documentation is available at **[isle.fedarovich.com](https://isle.fedarovich.com/)**:

+ [Introduction](https://isle.fedarovich.com/articles/introduction.html)
+ [Getting Started](https://isle.fedarovich.com/articles/getting-started.html)
+ Logging with [Microsoft.Extensions.Logging](https://isle.fedarovich.com/articles/microsoft-extensions-logging.html) and [Serilog](https://isle.fedarovich.com/articles/serilog.html)
+ [Argument Names](https://isle.fedarovich.com/articles/argument-names.html)
+ [Value Representation](https://isle.fedarovich.com/articles/value-representation.html)
+ [Literal Values](https://isle.fedarovich.com/articles/literal-values.html)
+ [Configuration](https://isle.fedarovich.com/articles/configuration.html)
+ [Roslyn Name Converter](https://isle.fedarovich.com/articles/roslyn-name-converter.html)
+ [Code Analysis](https://isle.fedarovich.com/articles/code-analysis.html)
+ [MSBuild Properties](https://isle.fedarovich.com/articles/msbuild-properties.html)
+ [How It Works](https://isle.fedarovich.com/articles/how-it-works.html)
+ [Migrating from v1.x to v2.x](https://isle.fedarovich.com/articles/migration-v2.html)
+ [API Reference](https://isle.fedarovich.com/api/Isle.Configuration.html)

## SAST Tools

[PVS-Studio](https://pvs-studio.com/en/pvs-studio/?utm_source=github&utm_medium=organic&utm_campaign=open_source) - static analyzer for C, C++, C#, and Java code.
