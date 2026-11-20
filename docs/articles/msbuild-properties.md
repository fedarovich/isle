# MSBuild Properties

The ISLE packages read these MSBuild properties. Set them in your project file or in `Directory.Build.props`.

## Global usings

Each integration package adds a `global using` directive to the projects that reference it, directly or indirectly. To turn one off, set its property to `False`:

| Property | Package | Global using it controls |
| -------- | ------- | ------------------------ |
| `UsingIsleExtensions` | `Isle.Core` | `Isle.Extensions` (the `Named` extension method) |
| `UsingIsleExtensionsLogging` | `Isle.Extensions.Logging` | `Isle.Extensions.Logging` (the `ILogger` extension methods) |
| `UsingIsleSerilog` | `Isle.Serilog` | `Isle.Serilog` (the Serilog `ILogger` extension methods) |

```xml
<PropertyGroup>
  <UsingIsleSerilog>False</UsingIsleSerilog>
</PropertyGroup>
```

With a global using turned off, add a `using` directive to the files that need it.

## Roslyn name converter analyzers

The [Isle.Converters.Roslyn.Analyzers](code-analysis.md#roslyn-name-converter-analyzers) package reads these properties. They tell the analyzers how your [Roslyn name converter](roslyn-name-converter.md) is configured, so the analyzers compute the same names as the converter does at run time.

| Property | Default | Description |
| -------- | ------- | ----------- |
| `IsleRoslynNameConverterRemoveMethodPrefixes` | *(empty)* | The method prefixes passed to [`RemoveMethodPrefixes`](roslyn-name-converter.md#removemethodprefixes). Separate them with semicolons, spaces or new lines. |
| `IsleRoslynNameConverterRemoveMethodPrefixesStringComparison` | `Ordinal` | The `StringComparison` passed to `RemoveMethodPrefixes`, for example `Ordinal` or `OrdinalIgnoreCase`. The value is case-insensitive. An invalid value silently falls back to `Ordinal`. |

For example, if the converter is configured like this:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithRoslynNameConverter(options => options
        .RemoveMethodPrefixes(["Get", "Is"], StringComparison.OrdinalIgnoreCase)
        .WithMemoization()));
```

then set:

```xml
<PropertyGroup>
  <IsleRoslynNameConverterRemoveMethodPrefixes>Get;Is</IsleRoslynNameConverterRemoveMethodPrefixes>
  <IsleRoslynNameConverterRemoveMethodPrefixesStringComparison>OrdinalIgnoreCase</IsleRoslynNameConverterRemoveMethodPrefixesStringComparison>
</PropertyGroup>
```

> [!TIP]
> Put these properties in `Directory.Build.props`, so every project that logs with ISLE gets the same values.
