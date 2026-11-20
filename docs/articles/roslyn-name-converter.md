# Roslyn Name Converter

The [built-in name converters](argument-names.md#built-in-converters) work on the raw text of the captured expression. <xref:Isle.Converters.Roslyn.RoslynNameConverter> goes further. It parses the expression with the C# compiler (Roslyn) and extracts the name of the member that produces the value:

| Expression | Name with `SerilogCompatible()` | Name with `RoslynNameConverter` |
| ---------- | ------------------------------- | ------------------------------- |
| `order.Customer.Name` | `OrderCustomerName` | `Name` |
| `GetTotal()` | `GetTotal` | `Total` (with `RemoveMethodPrefixes("Get")`) |
| `unchecked((short)value.IntProperty)` | `UncheckedshortvalueIntProperty` | `IntProperty` |
| `user?.Address?.City` | `UserAddressCity` | `City` |

The table assumes `CapitalizeFirstCharacter()` for both converters.

## Getting started

Install the package:

```bash
dotnet add package Isle.Converters.Roslyn
```

Add the converter to the configuration with `WithRoslynNameConverter`:

```csharp
using Isle.Configuration;

IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithRoslynNameConverter(options => options
        .CapitalizeFirstCharacter()
        .WithMemoization()));
```

The `options` parameter is an <xref:Isle.Configuration.IRoslynNameConverterConfigurationBuilder>. Its methods are described below.

> [!TIP]
> Install the [Isle.Converters.Roslyn.Analyzers](code-analysis.md#roslyn-name-converter-analyzers) package too. It reports expressions the converter can't handle, and duplicate names, at compile time.

## Configuration

### WithMemoization

```csharp
options.WithMemoization();                     // cache all results
options.WithMemoization(maxCacheSize: 10_000); // cache up to 10,000 results
```

> [!IMPORTANT]
> Memoization is **off** unless you call `WithMemoization`. Parsing is fast, but it still costs something on every logging call. Always turn memoization on for the Roslyn name converter.

### CapitalizeFirstCharacter

By default, the extracted name is not changed. `CapitalizeFirstCharacter()` makes the first letter upper-case, which gives `PascalCase` names (`customerId` → `CustomerId`).

### RemoveMethodPrefixes

When a value comes from a method call, the method name is used:

```csharp
logger.LogInformation($"The value is {GetValue()}.");  // {GetValue}
```

`RemoveMethodPrefixes` removes common prefixes from **method** names (property and variable names are never changed):

```csharp
options.RemoveMethodPrefixes("Get", "Is");   // GetValue() → Value, IsValid() → Valid
```

The comparison is ordinal and case-sensitive by default. Another overload takes a `StringComparison`:

```csharp
options.RemoveMethodPrefixes(["Get", "Is"], StringComparison.OrdinalIgnoreCase);
```

> [!NOTE]
> Prefixes are checked in the order you list them, and only the first match is removed. The check is a plain "starts with": with the prefix `Is`, `Isolate()` becomes `olate`. Choose prefixes that don't start other method names, or use a [custom transformation](#addtransformation).

If you use the [analyzers](code-analysis.md#roslyn-name-converter-analyzers), tell them about the prefixes with the [`IsleRoslynNameConverterRemoveMethodPrefixes`](msbuild-properties.md#roslyn-name-converter-analyzers) MSBuild property.

### AddTransformation

`CapitalizeFirstCharacter` and `RemoveMethodPrefixes` are built-in *transformations*. `AddTransformation` adds your own: a `Func<string, NameExpressionType, string>` that receives the current name and the kind of expression it came from (<xref:Isle.Converters.Roslyn.NameExpressionType>: `Identifier`, `Property` or `Method`).

```csharp
options
    .RemoveMethodPrefixes("Get")
    .AddTransformation((name, type) =>
        type == NameExpressionType.Method && name.EndsWith("Async", StringComparison.Ordinal)
            ? name[..^"Async".Length]
            : name)
    .CapitalizeFirstCharacter();
// GetOrderAsync() → OrderAsync → Order
```

> [!IMPORTANT]
> Transformations run **in the order they were added**, including `CapitalizeFirstCharacter` and `RemoveMethodPrefixes`. A leading `@` (as in `@point`) is removed before the transformations run and added back afterwards.

### WithFallback

Not every expression has a member name. For `2 + 2 * 2`, `nameof(Foo)` or `items[0]` the converter can't extract one, and it calls the *fallback* converter with the original expression text. The fallback's result is used as it is; the transformations are not applied to it.

The default fallback returns the expression unchanged. A common choice is `SerilogCompatible`:

```csharp
options.WithFallback(ValueNameConverters.SerilogCompatible());
```

The fallback is also used if parsing or a transformation throws an exception.

## Full configuration example

```csharp
using Isle.Configuration;
using Isle.Converters;

IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithRoslynNameConverter(options => options
        .CapitalizeFirstCharacter()
        .RemoveMethodPrefixes("Get")
        .WithMemoization()
        .WithFallback(ValueNameConverters.SerilogCompatible())));
```

## How it works

The converter parses the captured expression into a syntax tree and unwraps it step by step until it reaches a simple name. For example, in

```csharp
logger.LogInformation($"The int property is {unchecked((short)value.IntProperty)}");
```

it unwraps the `unchecked(...)` expression, the parentheses, the cast and the member access, and returns `IntProperty`.

### Supported expressions

These expressions are supported, in any valid combination:

| Expression | Example | Result |
| ---------- | ------- | ------ |
| identifier | `value`, `@value` | `value`, `@value` |
| member access | `obj.Property`, `this.Property`, `Type.StaticProperty` | `Property`, `StaticProperty` |
| conditional member access | `obj?.Property`, `obj?.Method()` | `Property`, `Method` |
| method invocation | `Method(a, b)`, `obj.GetValue()` | `Method`, `GetValue` |
| cast | `(short)value` | `value` |
| parentheses | `(value)` | `value` |
| increment and decrement | `++a`, `b--` | `a`, `b` |
| pointer indirection | `*ptr` | `ptr` |
| null-forgiving operator | `value!` | `value` |
| `checked` / `unchecked` | `checked(a)` | `a` |

### Unsupported expressions

These expressions don't produce a name, so the [fallback](#withfallback) is used. Give them an explicit name with [`Named`](argument-names.md#explicit-names-with-named):

- arithmetic and other binary or ternary operators: `a + b`, `x ?? y`, `c ? a : b`;
- element access: `items[0]`, `dict["key"]`;
- generic method calls: `Get<int>()`;
- `nameof(...)`, `typeof(...)`, `default`, literals, `new ...`, `await ...`, lambdas, `this` on its own.

> [!NOTE]
> The `@` operator survives only on a plain identifier. For `@order.Customer`, the name is `Customer`, without the `@`. To destructure such a value, use `order.Customer.Named("@Customer")`.

## Using the converter directly

`RoslynNameConverter` is a public class. You can create one and wrap its `Convert` method in your own converter:

```csharp
var roslyn = new RoslynNameConverter(
    fallback: ValueNameConverters.SerilogCompatible(),
    transformations: []);

IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithNameConverter(new Func<string, string>(roslyn.Convert).WithMemoization()));
```
