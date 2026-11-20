# Migrating from v1.x to v2.x

ISLE v2.x works with all your existing v1.x logging code. The breaking changes are all in the configuration. They are listed below, each with a fix.

## The configuration is not resettable by default

ISLE v1.x let you reset the configuration with `IsleConfiguration.Reset()` and then call `IsleConfiguration.Configure()` again with a new configuration.

Few applications need this; ISLE's own unit tests are one exception. It also prevents some performance optimizations (see [How It Works](how-it-works.md#configuration-as-jit-time-constants)). Starting from v2.x, the configuration is **not resettable by default**, and `Reset()` throws `InvalidOperationException`.

If you still need a resettable configuration, ask for it in **every** call to `Configure`:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .IsResettable());
```

See also: [Resettable vs Non-Resettable Configuration](configuration.md#resettable-vs-non-resettable-configuration).

## Integrations must be added explicitly

ISLE 1.x had the extension methods `ConfigureExtensionsLogging` and `ConfigureSerilog` to configure the integrations. Calling them was optional.

ISLE 2.x **requires** you to add each integration you use:

```csharp
// For Isle.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());

// For Isle.Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog());
```

Without it, the first log call at an enabled level throws `InvalidOperationException` ("Isle.Extension.Logging is not configured..." or "Isle.Serilog is not configured...").

Both methods take an optional delegate for integration options:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging(options => options.EnableMessageTemplateCaching = false));
```

> [!NOTE]
> Message template caching is **on by default**, so you don't need `EnableMessageTemplateCaching = true`. Set it to `false` only if you want to turn caching off.

To make migration easier, `ConfigureExtensionsLogging` and `ConfigureSerilog` still exist. They call `AddExtensionsLogging` and `AddSerilog`, but they are marked `[Obsolete]` and produce a compiler warning:

| Obsolete method | Replacement | Diagnostic ID |
| --------------- | ----------- | ------------- |
| `ConfigureExtensionsLogging` | `AddExtensionsLogging` | `ISLE1000` |
| `ConfigureSerilog` | `AddSerilog` | `ISLE2000` |

On `netstandard2.0` and .NET Framework, the warning is the generic CS0618.

## `IsleConfiguration.ValueNameConverter` is obsolete

The `IsleConfiguration.ValueNameConverter` property is obsolete. To convert a name with the configured converter, call `IsleConfiguration.Current.ConvertValueName(name)`. To set a converter, use [`WithNameConverter`](argument-names.md#name-converters) on the configuration builder, as before.

## New in v2.x

- The [Roslyn name converter](roslyn-name-converter.md) (`Isle.Converters.Roslyn`) and its [analyzers](code-analysis.md#roslyn-name-converter-analyzers) (`Isle.Converters.Roslyn.Analyzers`).
- Faster logging, thanks to the [non-resettable configuration](how-it-works.md#configuration-as-jit-time-constants).
