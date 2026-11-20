# Argument Names

ISLE names each property in the message template after the C# expression in the interpolation hole. It gets the expression text with [`CallerArgumentExpressionAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute):

```csharp
logger.LogInformation($"Area is {area}");            // {area}
logger.LogInformation($"Order {order.Id} shipped");  // {order.Id}
logger.LogInformation($"Total is {GetTotal()}");     // {GetTotal()}
```

Simple variable names work best. For anything more complex, you have three options:

1. give the value an [explicit name](#explicit-names-with-named) with `.Named("...")`;
2. configure a [name converter](#name-converters) that cleans up captured expressions;
3. use the [Roslyn name converter](roslyn-name-converter.md), which extracts the member name from almost any expression (`order.Id` → `Id`, `GetTotal()` → `Total`).

## Explicit names with `Named`

The <xref:Isle.Extensions.LoggingExtensions.Named*> extension method sets the property name yourself:

```csharp
int width = 5;
int height = 6;
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {(width * height).Named("area")}");
// Template: "... is {area}"
```

- `Named` works on any value. The `Isle.Extensions` namespace is added as a global using by the `Isle.Core` package.
- Alignment and format work as usual: `{total.Named("Total"),10:N2}`.
- The name can start with the `@` (destructure) or `$` (stringify) operator: `{order.Named("@Order")}`. See [Value Representation](value-representation.md).
- [Name converters](#name-converters) are **not** applied to explicit names. The name is used as written, except that a [value representation policy](value-representation.md#value-representation-policies) may add a prefix (see below).
- The name must not be null, empty or whitespace; otherwise `Named` throws `ArgumentException`.

> [!IMPORTANT]
> Always pass a **constant** to `Named`. Names are part of the [message template cache](configuration.md#template-caching), so dynamic names make the cache grow without limit. The parameter is marked with `[ConstantExpected]`, so the .NET analyzers report CA1857 for non-constant names, and the [ISLE analyzers](code-analysis.md) report ISLE4002.

### Explicit names and value representation

`Named` has a second overload, `Named(name, preserveDefaultValueRepresentation)`:

- `false`: if the name has no `@` or `$` prefix, the configured [value representation policy](value-representation.md#value-representation-policies) is applied, and it may add one based on the **compile-time** type of the value.
- `true`: the name is used exactly as written.

The overload without the boolean uses the global [`PreserveDefaultValueRepresentationForExplicitNames`](configuration.md#preservedefaultvaluerepresentationforexplicitnames) setting, which is `false` by default. So by default, explicit names get the same automatic prefixes as captured names.

## Name converters

A *name converter* is a `Func<string, string>` that ISLE applies to every captured expression before using it as a property name. Configure one with `WithNameConverter`:

```csharp
using Isle.Configuration;
using Isle.Converters;

IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithNameConverter(ValueNameConverters.SerilogCompatible().WithMemoization()));
```

### Built-in converters

The `Isle.Converters.ValueNameConverters` class provides these converters:

| Converter | What it does | Example |
| --------- | ------------ | ------- |
| `CapitalizeFirstCharacter()` | Makes the first character upper-case (invariant culture) if it is a lower-case letter. A leading `@` is skipped. | `order.Id` → `Order.Id`<br/>`@point` → `@Point` |
| `SerilogCompatible(capitalizeFirstCharacter: true)` | Removes every character except letters, digits and `_`, keeping a leading `@`. Optionally capitalizes the first letter. | `order.Id` → `OrderId`<br/>`GetTotal()` → `GetTotal`<br/>`@point` → `@Point` |

`SerilogCompatible` makes any expression a valid Serilog property name. Its results can be awkward (`items.Count()` → `ItemsCount`), and it throws `ArgumentException` if nothing is left after removing the invalid characters. The [Roslyn name converter](roslyn-name-converter.md) gives better names.

### Custom converters

Any `Func<string, string>` can be a converter:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithNameConverter(name => name.Replace('.', '_')));
```

A converter must:

- be **thread-safe**, because it is called concurrently from all logging threads;
- be **deterministic**, returning the same name for the same expression;
- never return an empty string.

If the converted name starts with `@` or `$`, the [value representation policy](value-representation.md#value-representation-policies) is not applied to it.

### Memoization

The converter runs on **every** logging call with an enabled level, even when template caching is on, because the converted name is part of the cache lookup. Wrap the converter with `WithMemoization` to cache its results:

```csharp
// Cache all results
ValueNameConverters.CapitalizeFirstCharacter().WithMemoization()

// Cache at most 1000 results; expressions beyond that are converted on every call
ValueNameConverters.CapitalizeFirstCharacter().WithMemoization(maxCacheSize: 1000)
```

Captured expressions come from your source code, so there are only so many of them, and unbounded memoization is usually safe. `WithMemoization(0)` turns memoization off and returns the original converter.
