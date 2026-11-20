# Code Analysis

ISLE comes with Roslyn analyzers that catch problems at compile time.

## ISLE0001

| ID | Severity | Package |
| -- | -------- | ------- |
| ISLE0001 | Error | `Isle.Core` (installed with every integration) |

**C# language version is less than 10.0.** ISLE relies on interpolated string handlers and `CallerArgumentExpressionAttribute`, both of which need C# 10. The analyzer reports an error if the project uses an older language version, which is common in .NET Framework and .NET Standard projects. To fix it, use the .NET 6 SDK or later and set the language version in the project file:

```xml
<PropertyGroup>
  <LangVersion>latest</LangVersion>
</PropertyGroup>
```

## Roslyn name converter analyzers

The [Roslyn name converter](roslyn-name-converter.md) can't extract a name from every C# expression. When it can't, the [fallback](roslyn-name-converter.md#withfallback) is used, which is rarely what you want. A related problem: two holes can end up with the same name:

```csharp
logger.LogInformation($"{a.X} {b.X}");   // both properties are named "X"
```

The `Isle.Converters.Roslyn.Analyzers` package finds both problems, and a few more, in every call to an `Isle.Extensions.Logging` or `Isle.Serilog` logging method, including `BeginScopeInterpolated`.

### Installation

If you use `Isle.Converters.Roslyn`, install the analyzers in **every project that logs with ISLE**, not only in the project that configures it. The easiest way is a global reference in `Directory.Packages.props` with [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management):

```xml
<ItemGroup>
  <!-- Use the latest version of the package -->
  <GlobalPackageReference Include="Isle.Converters.Roslyn.Analyzers" Version="2.0.0" />
</ItemGroup>
```

Or add it to `Directory.Build.props`:

```xml
<ItemGroup>
  <PackageReference Include="Isle.Converters.Roslyn.Analyzers" Version="2.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildTransitive</IncludeAssets>
  </PackageReference>
</ItemGroup>
```

The package is a development dependency: it adds nothing to your build output and isn't passed on to projects that depend on yours.

If you use [`RemoveMethodPrefixes`](roslyn-name-converter.md#removemethodprefixes), set the same prefixes in the [`IsleRoslynNameConverterRemoveMethodPrefixes`](msbuild-properties.md#roslyn-name-converter-analyzers) MSBuild property. The analyzers need them to compute the same names as the converter.

### Diagnostics

All diagnostics are warnings in the *Usage* category, and all are enabled by default.

| ID | Message | Code fix |
| -- | ------- | -------- |
| ISLE4000 | Captured name '{0}' cannot be converted to a valid C# identifier. | Wraps the expression as `(expression).Named("TODO_SPECIFY_NAME")`. |
| ISLE4001 | Captured name '{0}' is already used in this message template. | none |
| ISLE4002 | Non-constant name is passed to the Named method. | none |
| ISLE4003 | Explicit name '{0}' is not a valid C# identifier. | none |
| ISLE4004 | Explicit name '{0}' has leading or trailing white spaces. | Trims the name. Supports *Fix All*. |

#### ISLE4000: captured name cannot be converted

The expression isn't one of the [supported expressions](roslyn-name-converter.md#supported-expressions), for example `a + b`, `items[0]` or `nameof(Foo)`. Give the value an explicit name:

```csharp
// warning ISLE4000
logger.LogInformation($"Sum is {a + b}");

// fixed
logger.LogInformation($"Sum is {(a + b).Named("Sum")}");
```

To put a `nameof(...)` result into the template text, use a [`LiteralValue`](literal-values.md).

#### ISLE4001: captured name is not unique

Two holes in the same message would get the same property name. The comparison is case-insensitive, ignores a leading `@` or `$`, and takes the configured method prefixes into account:

```csharp
// warning ISLE4001
logger.LogInformation($"Moved from {from.X} to {to.X}");

// fixed
logger.LogInformation($"Moved from {from.X.Named("FromX")} to {to.X.Named("ToX")}");
```

#### ISLE4002: explicit name is not a constant

The name passed to `Named` isn't a compile-time constant. Names are part of the [template cache](configuration.md#template-caching), so dynamic names cause unbounded memory growth. Use a string literal or a `const`.

#### ISLE4003: explicit name is not a valid C# identifier

The name passed to `Named`, without its leading `@` or `$`, isn't a valid C# identifier. That usually means it isn't a valid property name for the logging framework either.

#### ISLE4004: explicit name has leading or trailing white space

The name passed to `Named` has leading or trailing white space. The code fix removes it.

### Changing severity

Use `.editorconfig` to change the severity of any diagnostic, as for any Roslyn analyzer:

```ini
[*.cs]
dotnet_diagnostic.ISLE4001.severity = error
dotnet_diagnostic.ISLE4003.severity = suggestion
```
