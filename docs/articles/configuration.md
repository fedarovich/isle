# Configuration

## Basics

Configure ISLE with <xref:Isle.Configuration.IsleConfiguration.Configure*> before you call any ISLE logging method:

```csharp
using Isle.Configuration;

IsleConfiguration.Configure(builder => { /* configure here */ });
```

The delegate receives an <xref:Isle.Configuration.IIsleConfigurationBuilder>. You can set its properties directly, or chain the fluent extension methods described below.

> [!IMPORTANT]
> - Call `Configure` **exactly once**. A second call throws `InvalidOperationException`, unless the configuration is [resettable](#resettable-vs-non-resettable-configuration) and was reset.
> - Call it **as early as possible**: at the top of `Main`/`Program.cs`, or in a module initializer of your executable. See [Getting Started](getting-started.md#where-to-put-the-configuration).
> - Configure ISLE in the application, not in libraries.

After configuration, <xref:Isle.Configuration.IsleConfiguration.Current?displayProperty=nameWithType> returns a read-only view of the settings. It throws `InvalidOperationException` if ISLE is not configured.

## Fluent configuration methods

| Method | Package | Description |
| ------ | ------- | ----------- |
| `AddExtensionsLogging(options?)` | `Isle.Extensions.Logging` | Adds the [Microsoft.Extensions.Logging](microsoft-extensions-logging.md) integration. |
| `AddSerilog(options?)` | `Isle.Serilog` | Adds the [Serilog](serilog.md) integration. |
| `WithNameConverter(converter)` | `Isle.Core` | Sets the [name converter](argument-names.md#name-converters). |
| `WithRoslynNameConverter(options)` | `Isle.Converters.Roslyn` | Sets the [Roslyn name converter](roslyn-name-converter.md). |
| `WithValueRepresentationPolicy(policy)` | `Isle.Core` | Sets the [value representation policy](value-representation.md#value-representation-policies). |
| `WithAutomaticDestructuring()` | `Isle.Core` | Same as `WithValueRepresentationPolicy(AutoDestructuringValueRepresentationPolicy.Instance)`. |
| `IsResettable(bool isResettable = true)` | `Isle.Core` | Makes the configuration [resettable](#resettable-vs-non-resettable-configuration). |

The methods return the builder, so you can chain them:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithNameConverter(ValueNameConverters.SerilogCompatible().WithMemoization()));
```

Some properties have no fluent method. Set them in a block lambda:

```csharp
IsleConfiguration.Configure(builder =>
{
    builder.AddExtensionsLogging();
    builder.PreserveDefaultValueRepresentationForExplicitNames = true;
    builder.CacheLiteralValues = false;
});
```

## Builder properties

### ValueRepresentationPolicy

Type: <xref:Isle.Configuration.IValueRepresentationPolicy>. Default: `null`, which means <xref:Isle.Configuration.DefaultValueRepresentationPolicy>, so the logging framework chooses the representation.

The policy can prepend the destructuring (`@`) or stringification (`$`) operator to argument names, based on the **compile-time** type of the value. See [Value Representation](value-representation.md).

### ValueNameConverter

Type: `Func<string, string>`. Default: `null`, so names are used exactly as captured.

The converter is applied to every name captured from a C# expression. It is not applied to explicit names set with `Named(...)`. See [Argument Names](argument-names.md#name-converters) and [Roslyn Name Converter](roslyn-name-converter.md).

### PreserveDefaultValueRepresentationForExplicitNames

Type: `bool`. Default: `false`.

Controls the `Named(name)` overload, the one without the `preserveDefaultValueRepresentation` argument:

- `false` (default): the value representation policy **is applied** to explicit names that don't start with `@` or `$`, the same as for captured names.
- `true`: explicit names are used **exactly as written**.

The `Named(name, preserveDefaultValueRepresentation)` overload ignores this setting.

### CacheLiteralValues

Type: `bool`. Default: `false`.

Sets whether a [literal value](literal-values.md) created with the one-argument `LiteralValue(string)` constructor, or with the explicit cast from `string`, takes part in template caching.

> [!CAUTION]
> Turning this on can cause memory leaks if any literal value is not constant. Leave it off and use the `LiteralValue(string, bool)` constructor to decide for each literal.

### IsResettable

Type: `bool`. Default: `false`. See [Resettable vs Non-Resettable Configuration](#resettable-vs-non-resettable-configuration).

## Adding integrations

Every integration you use **must** be added to the configuration:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()   // Isle.Extensions.Logging
    .AddSerilog());           // Isle.Serilog
```

Add each integration only once. Both methods take an optional delegate for integration-specific options. Today the only option is `EnableMessageTemplateCaching`.

## Template caching

Every time an ISLE method logs at an enabled level, the message template has to be produced from the parts of the interpolated string. ISLE can do this in two ways:

- **With caching (default):** ISLE stores templates in a process-wide, lock-free, trie-like cache, keyed by the literal parts and the names, alignments and formats of the holes. A template is built once and then found in the cache. For Serilog, the parsed `MessageTemplate` object is cached too. This is much faster and produces less garbage, at the cost of the memory the cached templates use.
- **Without caching:** the template string is built from scratch on every call. It uses the least memory, but is slower and allocates more.

Caching is **on by default** for both integrations. Turn it off only if memory is very tight:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging(options => options.EnableMessageTemplateCaching = false)
    .AddSerilog(options => options.EnableMessageTemplateCaching = false));
```

Cache entries are never removed. That's fine for templates written in source code, because there are only so many of them. It's a problem if names or cacheable literals are dynamic:

- pass only constants to [`Named`](argument-names.md#explicit-names-with-named);
- mark a [`LiteralValue`](literal-values.md) as cacheable only if its text is constant or comes from a small, fixed set.

## Resettable vs non-resettable configuration

By default the configuration is **non-resettable**: once ISLE is configured, it can't be changed for the life of the process. This lets ISLE store the settings in `static readonly` fields. The JIT treats those as constants, so it can remove unused code paths and call the configured policy and builders directly (devirtualization). See [How It Works](how-it-works.md#configuration-as-jit-time-constants).

A **resettable** configuration can be cleared with <xref:Isle.Configuration.IsleConfiguration.Reset*?displayProperty=nameWithType> and configured again. Every configuration in the process must ask to be resettable:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .IsResettable());

// later
IsleConfiguration.Reset();
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithAutomaticDestructuring()
    .IsResettable());
```

`Reset()` throws `InvalidOperationException` if the configuration is not resettable. If ISLE isn't configured, it does nothing.

> [!NOTE]
> The mode is fixed for the whole process the first time ISLE reads its settings. If ISLE code reads the settings before `Configure` is called, for example through an enabled-level log call or `LiteralValue(string)`, the process stays on the slower resettable path even after a non-resettable configuration. Logging still works correctly. Configuring ISLE first avoids this.

> [!NOTE]
> Non-resettable configuration is faster in synthetic benchmarks. With some workloads the difference may be negligible. Measure with a profiler if performance matters to you.

## Configuring ISLE in unit tests

ISLE's configuration is process-wide, and a test runner usually runs all tests of an assembly in one process. So configure ISLE **once per test assembly**:

```csharp
// NUnit
[SetUpFixture]
public class AssemblySetup
{
    [OneTimeSetUp]
    public void Configure() => IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
}

// MSTest
[TestClass]
public static class AssemblySetup
{
    [AssemblyInitialize]
    public static void Configure(TestContext _) => IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
}
```

With xUnit, or with any framework, a `[ModuleInitializer]` in the test project works too.

If different tests need different configurations, make every configuration resettable and reset it in teardown:

```csharp
[SetUp]
public void SetUp() => IsleConfiguration.Configure(builder => builder.AddExtensionsLogging().IsResettable());

[TearDown]
public void TearDown() => IsleConfiguration.Reset();
```

These tests can't run in parallel with each other.

## Sample configurations

`Microsoft.Extensions.Logging` with Serilog as the provider, using the Roslyn name converter:

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

Serilog directly, with a simple name converter:

```csharp
using Isle.Configuration;
using Isle.Converters;

IsleConfiguration.Configure(builder => builder
    .AddSerilog()
    .WithNameConverter(ValueNameConverters.SerilogCompatible().WithMemoization()));
```

## Writing your own integration

The integrations hook into configuration through <xref:Isle.Configuration.IIsleConfigurationBuilder.RegisterExtensionConfigurationHook*>. A hook implements <xref:Isle.Configuration.IIsleExtensionConfigurationHook>:

- `ApplyExtensionConfiguration()` is called after the core configuration has been applied;
- `ResetExtensionConfiguration()` is called by `IsleConfiguration.Reset()`.

Most applications never need this. It is for authors of integrations with other logging libraries.
