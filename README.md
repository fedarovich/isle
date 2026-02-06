# ISLE

![Isle logo](/IsleLogo.png)

ISLE (Interpolated String Logging Extensions) is a library that allows developers to perform structured logging using interpolated strings in C# 10 or later.

| Package | Release | Prerelease | Dev Build | Description |
| ------- | ---------- | ------- | --------- | ----------- |
| Isle.Core | [![nuget](https://img.shields.io/nuget/v/Isle.Core.svg?label=nuget)](https://www.nuget.org/packages/Isle.Core) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Core.svg?label=nuget)](https://www.nuget.org/packages/Isle.Core) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Core.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Core) | Core package containing shared logic. |
| Isle.Extensions.Logging | [![nuget](https://img.shields.io/nuget/v/Isle.Extensions.Logging.svg?label=nuget)](https://www.nuget.org/packages/Isle.Extensions.Logging) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Extensions.Logging.svg?label=nuget)](https://www.nuget.org/packages/Isle.Extensions.Logging) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Extensions.Logging.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Extensions.Logging) | Integration package for Microsoft.Extensions.Logging. |
| Isle.Serilog | [![nuget](https://img.shields.io/nuget/v/Isle.Serilog.svg?label=nuget)](https://www.nuget.org/packages/Isle.Serilog) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Serilog.svg?label=nuget)](https://www.nuget.org/packages/Isle.Serilog) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Serilog.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Serilog) | Integration package for Serilog. |
| Isle.Converters.Roslyn | [![nuget](https://img.shields.io/nuget/v/Isle.Converters.Roslyn.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Converters.Roslyn.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Converters.Roslyn.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Converters.Roslyn) | Roslyn-based name converter. |
| Isle.Converters.Roslyn.Analyzers | [![nuget](https://img.shields.io/nuget/v/Isle.Converters.Roslyn.Analyzers.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn.Analyzers) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Converters.Roslyn.Analyzers.svg?label=nuget)](https://www.nuget.org/packages/Isle.Converters.Roslyn.Analyzers) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Converters.Roslyn.Analyzers.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Converters.Roslyn.Analyzers) | Code analyzers for Roslyn-based name converter. |
| Isle.Compatibility | [![nuget](https://img.shields.io/nuget/v/Isle.Compatibility.svg?label=nuget)](https://www.nuget.org/packages/Isle.Compatibility) | [![nuget](https://img.shields.io/nuget/vpre/Isle.Compatibility.svg?label=nuget)](https://www.nuget.org/packages/Isle.Compatibility) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Compatibility.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Compatibility) | Compatibility package for `netstandard2.0` and `netstandard2.1`. |

[![Build Status](https://dev.azure.com/pavelfedarovich/ISLE/_apis/build/status/fedarovich.isle?branchName=main)](https://dev.azure.com/pavelfedarovich/ISLE/_build/latest?definitionId=12&branchName=main)
[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/fedarovich/isle/blob/master/LICENSE)

ISLE works on any modern .Net version and also supports .Net Standard 2.0, thus can be used with .Net Framework 4.6.2 or later.

> [!IMPORTANT]
> ISLE v2.x introduces some breaking changes in its configuration. If you are using ISLE v1.x and want to upgrade to v2.x, please see the section [Migrating from v1.x to v2.x](#migrating-from-v1x-to-v2x) below.

## Table of Contents

+ [Why ISLE?](#why-isle)
+ [Getting Started](#getting-started)
+ [Value Representation](#value-representation)
+ [Custom Argument Names](#custom-argument-names)
+ [Literal Values](#literal-values)
+ [Configuring ISLE](#configuring-isle)
  + [Basics](#basics)
  + [Adding Intergrations](#adding-integrations)
  + [Template Caching](#template-caching)
  + [Resettable vs Non-Resettable Configuration](#resettable-vs-non-resettable-configuration)
+ [Roslyn Name Converter](#roslyn-name-converter)
  + [Getting Started with Roslyn Name Converter](#getting-started-with-roslyn-name-converter)
  + [Full Configuration Example](#full-configuration-example)
  + [How It Works](#how-it-works)
+ [Migrating from v1.x to v2.x](#migrating-from-v1x-to-v2x)

## Why ISLE?

+ ISLE allows you to use C# interpolated strings in loggers instead of passing a message template and arguments separately as in `string.Format`.
+ ISLE is fast. With template caching enabled it can be faster compared to using standard logging methods, as it eliminates the template parsing.
+ ISLE logging methods are no-op for disabled log levels. If a log level is disabled, the template argument values will not be evaluated and the underlying `Microsoft.Extensions.Logging.ILogger.Log` or `Serilog.ILogger.Write` method won't be called. To achieve the same with the standard logging methods you have to wrap each call with `if` checking whether the log level is enabled.

## Getting Started

ISLE provides a set of extension methods to be used in combination with well-known logging libraries. The following extensions are supported at the moment:

| Logging Library | Target Interface | Package | Extension Methods |
| --------------- | ---------------- | ------- | ----------------- |
| Microsoft.Extensions.Logging | [`Microsoft.Extensions.Logging.ILogger`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) | [![nuget](https://img.shields.io/nuget/v/Isle.Extensions.Logging.svg?label=Isle.Extensions.Logging)](https://www.nuget.org/packages/Isle.Extensions.Logging) | `Log` <br/> `LogTrace` <br/> `LogDebug` <br/> `LogInformation` <br/> `LogWarning` <br/> `LogError` <br/> `LogCritical` <br/> `BeginScopeInterpolated` |
| Serilog | `Serilog.ILogger` | [![nuget](https://img.shields.io/nuget/v/Isle.Serilog.svg?label=Isle.Serilog)](https://www.nuget.org/packages/Isle.Serilog) | `WriteInterpolated` <br/> `VerboseInterpolated` <br/> `DebugInterpolated` <br/> `InformationInterpolated` <br/> `WarningInterpolated` <br/> `ErrorInterpolated` <br/> `FatalInterpolated` |

In order to perform structured logging you must also add and configure the underlying logging provider (e.g. [Serilog](https://serilog.net)).

To begin using ISLE you must install the corresponding package from NuGet.

```bash
dotnet add package Isle.Extensions.Logging
# AND/OR
dotnet add package Isle.Serilog
```

> [!TIP]
> If you are using Serilog only indirectly via `Microsoft.Extensions.Logging.ILogger` facade, there is no need to install `Isle.Serilog`. Install just `Isle.Extensions.Logging` package instead.

Next, at the beginning of your program you must configure ISLE. The simplest configuration looks like this:
```cs
// For use with Microsoft.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging(opt => opt.EnableMessageTemplateCaching = true));
// or for use with Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog(opt => opt.EnableMessageTemplateCaching = true));
```
> [!IMPORTANT]
> The `Configure` method **must** be called before calling any of the ISLE extension methods for the first time, thus you should place it either at the beginning of your program, or to the module initializer of your executable assembly. Note that you must call `Configure` **exactly once** unless resettable configuration is being used, the subsequent calls with throw `InvalidOperationException`. See the [Configuring ISLE](#configuring-isle) section below for more details.

Now you can simply pass interpolated strings to the ILogger's methods and ISLE will capture the variables names and make the correct template string for the structured logging.

Consider the following example without using ISLE:
```cs
int width = 5;
int height = 6;
int area = width * height;

// Microsoft.Extensions.Logging
logger.LogInformation("The area of rectangle with the width = {width} and the height = {height} is {area}", width, height, area);

// Serilog
logger.Information("The area of rectangle with the width = {width} and the height = {height} is {area}", width, height, area)l
```
As you can see, we had to duplicate the names of the variables in the template message string.

ISLE will do it for you:
```cs
int width = 5;
int height = 6;
int area = width * height;

// Microsoft.Extensions.Logging
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {area}");

// Serilog
logger.InformationInterpolated($"The area of rectangle with the width = {width} and the height = {height} is {area}")
```
This code with ISLE installed will produce exactly the same structured log message as previous one.

> [!NOTE]
> While Isle.Extensions.Logging is providing the same method names as `Microsoft.Extensions.Logging.ILogger` itself (e.g. `LogError`), the Isle.Serilog's method names have suffix *`Interpolated`* (e.g. `ErrorInterpolated`) because of the way these interfaces are defined and how overload resolution works in C#.

## Value Representation
Any argument you log will be serialized by the logging framework either into its original (for primitives like strings, numbers and booleans), string or destructured (object) form.

By default, ISLE pass your argument names as is, so that the underlying logging framework selects the representation. For example, Serilog by default uses primitive or string representation for any argument unless its name is preceeded by the **at** sign (`@`) or has an explicitly configured destructurer for its type. Luckily, you can preceed any C# identifier with `@`.

Consider the following code:
```cs
public record Point(int X, int Y);

var record = new Record(3, 5);

// Use string form
logger.LogInformation($"The point is {point}.");

// Use destructured form:
logger.LogInformation($"The point is {@point}.");
```

Alternatively, ISLE allows you to specify value representation based on its **compile-time** type.

For example, to automatically destructure all non-scalar type and collections, we can configure ISLE in the following way:
```cs
IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring());
```
This code will apply `AutoDestructuringValueRepresentationPolicy` which stringifies the scalar types (e.g. numbers, strings, dates) but destructures custom objects and collections.

> [!CAUTION]
> As the destructured representation can be large, excessive and might contain the properties that have duplicate data or that must not be logged for security/compliance reasons, it's usually better to keep the automatic destructuring disabled, and either prefix the arguments with `@` where needed, or configure your underlying logging framework (e.g. Serilog) to automatically destructure only some concrete types in a way you need it.

You can also create your own custom value representation policy by implementing `IValueRepresentationPolicy` and configuring ISLE to use it:
```cs
IsleConfiguration.Configure(builder => builder.WithValueRepresentationPolicy(new YourCustomPolicy()));
```

## Custom Argument Names
ISLE automatically captures argument names by using C# 10 `CallerArgumentExpressionAttribute`. Thus, it is recommended that you use simple variable or property names as the arguments when possible.

You can also set a custom name for the argument using the `Named` extension method:
```cs
int width = 5;
int height = 6;
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {(width * height).Named("area")}");
```

> [!TIP]
> The `Named` extension method has an overload that takes an additional boolean parameter `preserveDefaultValueRepresentation`. If `true`, the name will be preserved as is; if `false` the name might be prepended with `@` (for destructuring) or `$` (for stringification) depending on the value **compile-time** type and the configured [`IsleConfiguration.ValueRepresentationPolicy`](#value-representation). Using the overload without this boolean parameter is equivalent to passing `IsleConfiguration.PreserveDefaultValueRepresentationForExplicitNames` as this parameter.

Some logging frameworks, e.g. Serilog, has some limitations for argument names. In order to address it, you can configure a transform to be applied to the names automatically captured by ISLE. In order to this, create a transformation method and pass it as delegate when configuring ISLE:
```cs
IsleConfiguration.Configure(builder => builder.WithNameConverter(ValueNameConverters.SerilogCompatible(capitalizeFirstCharacter: true)));
```

> [!TIP]
> Please note that the transform is not applied to the names explicitly specified using the `Named` method.

You can find some built-in name converters in the class `Isle.Converters.ValueNameConverters`:
+ `CapitalizeFirstCharacter()` - capitalizes the first character using the invariant culture
+ `SerilogCompatible(bool capitalizeFirstCharacter = true)` - removes all characters from the name that are not letters, numbers or underscore (`_`), and optionally capitalizes the first letter to match Serilog's best practices.

Normally, value converters produce a new string for each. While it is fast, it produces additional garbage for GC to collect. For this reason, `Isle.Converters.ValueNameConverters` also provides extension methods that can be applied to any converter to perform memoization of the convertion results:
```cs
IsleConfiguration.Configure(builder => builder.WithNameConverter(ValueNameConverters.CapitalizeFirstCharacter().WithMemoization()));
```

> [!IMPORTANT]
> Starting from version 2.x, ISLE provides a new powerful Roslyn-based name converter that is shipped as a separate package `Isle.Converters.Roslyn`. See the corresponding section below about how to use it.

## Literal values
In rare cases you might want some value in the interpolated string to become a part of the message template instead of being treated as an argument. Consider the following example:
```cs
logger.LogWarning($"Retrieved the {@value} from the {nameof(LocalStorageConfigSource)}."); 
```

Here we want to pass the `value` as an argument, however `nameof(LocalStorageConfigSource)` is a compile-time constant, so we would prefer it to be just the part of the message template. It can be achieved by wrapping it into a `LiteralValue` struct:
```cs
logger.LogWarning($"Retrieved the {@value} from the {new LiteralValue(nameof(LocalStorageConfigSource), true)}.")
```
The constructor of the `LiteralValue` accepts two parameters: the string to be used as a part of the message template, and a boolean indicating whether this value must participate in template caching. There is another constructor overload with one parameter, that simply calls the former overload passing `IsleConfiguration.CacheLiteralValues` as the second argument value.

> [!CAUTION]
> Use caching only for literal values that either:
> + compile-time or runtime constants (e.g. the result of `nameof(Something)`)
> + or have a very small number of possible values (e.g. a boolean or small enum).
>
> Otherwise, you will get a **memory leak** in your application.

## Configuring ISLE

### Basics
In order use ISLE, you must configure it before calling any of its extension methods. The configuration is done by calling the `IsleConfiguration.Configure` method and passing a delegate to it to perform the actual configuration.

```cs
IsleConfiguration.Configure(builder => { /* Do the configuration */ });
```

> [!IMPORTANT]
> The `Configure` method must be called **exactly once** unless resettable configuration is being used, the subsequent calls with throw `InvalidOperationException`. Thus, it is highly recommended to put it either at the beginning of your application's `Main` method, or into a module initializer. For unit test projects, you can put is either into an assembly-level one time setup method of your logging framework, or into a module initializer. You can also consider using resettable configuration in unit tests, but it's usually not necessary.

The `builder` parameter is an instance of `IIsleConfigurationBuilder` that provides a set of properties you can use to configure ISLE. There is also a bunch of extension methods that can be chained to provide a fluent interface for the configuration.

#### ValueRepresentationPolicy
The `IIsleConfigurationBuilder.ValueRepresentationPolicy` property allows you to set a `IValueRepresentationPolicy` that controls how the values of certain **compile-time** types will be [represented](#value-representation). By default, the no explicit representation is set, so it's up to the logging framework to decide how to represent the value (as string, number, object, etc.). A custom policy, however, may prepend a destructuring (`@`) or stringification (`$`) operator to the argument name in order to ask the logging framework to use a certain representation.

> [!NOTE]
> ISLE does not do any kind of destructuring or stringification itself, it just provides a tip to the underlying logging framework. It is the responsibility of the logging framework to recognize the operators and use the correct representation. For example, these operators are supported by Serilog both when used directly and through Microsoft.Extensions.Logging facade.

There is a built-in `AutoDestructuringValueRepresentationPolicy` that keeps a default representation for scalar types (numbers, strings, booleans, URIs, GUIDs, dates and times), but applies desctructuring to all other types. You can use an extension method to enable this policy:
```cs
IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring());
```

You can also apply your own policy using `WithValueRepresentationPolicy` extension method:
```cs
 // To use a built-in AutoDestructuringValueRepresentationPolicy
// or
IsleConfiguration.Configure(builder => builder.WithValueRepresentationPolicy(new MyPolicy()));
```

> [!CAUTION]
> As the destructured representation can be large, excessive and might contain the properties that have duplicate data or that must not be logged for security/compliance reasons, it's usually better to keep the automatic destructuring disabled, and either prefix the arguments with `@` where needed, or configure your underlying logging framework (e.g. Serilog) to automatically destructure only some concrete types in a way you need it.

#### ValueNameConverter

The `IIsleConfigurationBuilder.ValueNameConverter` property allows you to set a [value name converter](#custom-argument-names) that will be applied to all argument names. By default no converter is set, so the argument names are exactly the pieces of C# code passed as the arguments. However, if you use arbitrary C# expressions as the arguments, the names might be badly readable or even unsupported by the logging framework (e.g. Serilog does not allow anything but letters, numbers and underscores). And this is where the value name converters are handy. For example, if you use Serilog, you can use a converter to make the names compatible with it by removing all characters except letters, numbers and underscores:
```cs
IsleConfiguration.Configure(builder => builder.WithNameConverter(ValueNameConverters.CapitalizeFirstCharacter().WithMemoization()));
```

Starting from version 2.x, ISLE provides a new powerful Roslyn-based name converter that is shipped as a separate package `Isle.Converters.Roslyn`. See the corresponding section below about how to use it.

#### PreserveDefaultValueRepresentationForExplicitNames

The `IIsleConfigurationBuilder.PreserveDefaultValueRepresentationForExplicitNames` property controls whether the `ValueRepresentationPolicy` is applied to the argument names explicitly specified by using the `.Named("SomeName")` extension method. The default value is `false`, so the explicit names remain unchanged even if there is a `ValueRepresentationPolicy` set. If you change the value to `true`, the `ValueRepresentationPolicy` will be evaluated for explicit names without an operator (`@` or `$`) and an operator will be prepended if it is required by the policy.

#### CacheLiteralValues
The `IIsleConfigurationBuilder.CacheLiteralValues` property controls whether the message templates with [Literal Values](#literal-values) should be cached by default when [template caching](#template-caching) is enabled. The default value is `false`. The setting is used only when the `LiteralValue` contructor is invoked with a single argument. It is recommended to use the two argument constructor to specify caching rule for each concrete case.

> [!CAUTION]
> Having `CacheLiteralValues` enabled can lead to memory leaks when used without enough caution. Prefer having it disabled globally and use the two argument constructor of `LiteralValue` to specify caching rule for each concrete case.

#### IsResettable
The `IIsleConfigurationBuilder.IsResettable` property controls whether the configuration is resettable. By default, the configuration is not resettable so the `Reset` method will throw `InvalidOperationException` and, thus, ISLE cannot be reconfigured. You can make the configuration resettable by using the extension method:
```cs
IsleConfiguration.Configure(builder => builder.IsResettable());
```
See more details on Resettable vs Non-Resettable configuration below.

### Adding Integrations
In order to use integrations, such as Isle.Extensions.Logging and Isle.Serilog, they **must** be added to configuration:
```cs
// For Isle.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
// For Isle.Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog());
```
The methods `AddExtensionsLogging` and `AddSerilog` has an optional parameter that accept a configuration delegate. It can currently be used to configure template caching (see the next section).

### Template Caching
By default, each time you call ISLE's logging method, it will use an interpolated string handler to build a message template from scratch. While being highly optimized, this operation still produces some overhead, so, unless you have very string memory constraints, it's highly recommended to enable message template caching:
```cs
// For Isle.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging(opt => opt.EnableMessageTemplateCaching = true));
// For Isle.Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog(opt => opt.EnableMessageTemplateCaching = true));
```
When message template caching is enabled, the interpolated string handler will internally use an optimized trie-like structure to cache the message templates. It can significantly improve the logging method execution time and also reduces the load on GC, though slightly increases the total memory being used by the process.

### Resettable vs Non-Resettable Configuration

Starting from version 2.0, ISLE uses non-resettable configuration by default. It means that one ISLE is configured, you cannot use the `Reset` method to reset the configuration and reconfigure ISLE in a different way. The motivation behind it is to make some parts of the configuration JIT-time constants and allow JIT to perform some additional optimizations.

If you for some reason want to be able to configure ISLE, you can still make the configuration resettable by setting [`IsResettable`](#isresettable) property to true during the initial and subsequent `Configure` method calls.

> [!NOTE]
> While non-resettable configuration is faster in syntetic benchmarks, there is a tiny chance that under some load profiles resettable configuration might be better. Please, use some profiling tools if you have any concerns.

### Sample Configuration

Here is a full sample configuration for using Microsoft.Extensions.Logging with Serilog backend:
```cs
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging(opt => opt.EnableMessageTemplateCaching = true)
    .WithNameValueConverter(ValueNameConverters.SerilogCompatible()));
```

## Roslyn Name Converter

### Getting Started with Roslyn Name Converter

Starting from version 2.0, ISLE comes with `RoslynNameConverter` - a powerful [value name converter](#custom-argument-names) using Roslyn to extract the argument names from the C# expressions. It is shipped as a separate NuGet package that can be installed using your IDE or by the following command:
```bash
dotnet add package Isle.Converters.Roslyn
```

Next, you can add it to your ISLE configuration:
```cs
IsleConfiguration.Configure(builder => builder.WithRoslynNameConverter(opt => { /* Configure converter here */ }));
```

Now, it is important to set a few configure for the converter itself. The configuration can be done using a few extension methods, that can be called on the delegate parameter called `opt` in the sample above.

#### CapitalizeFirstCharacter
By default, the converter keep the name untouched. You can add `opt.CapitalizeFirstCharacter()` to enable capitalization of the first character, so that the argument name is always in `PascalCase`.

#### RemoveMethodPrefixes
Let's consider the following example
```cs
logger.LogInformation($"The value is {GetValue()}.");
```

The argument name will be captured as `GetValue` by default, however it would be better to capture it as `Value`. It is possible, of cause, to use `.Named("Value")` to specify the name manually, but `RoslynNameConverter` provides a better solution for it. You can configure removal of certain method prefixes by using `RemoveMethodPrefixes` methods:
```cs
IsleConfiguration.Configure(builder => builder.WithRoslynNameConverter(opt => opt.RemoveMethodPrefixes("Get", "Is")));
```

This method is using case sensitive ordinal comparison by default, but you can use another overload to specify the comparison method. E.g. to use ordinal case insensitive comparison, you can configure it in the following way:
```cs
IsleConfiguration.Configure(builder => builder.WithRoslynNameConverter(opt => opt.RemoveMethodPrefixes(["Get", "Is"], StringComparison.OrdinalIgnoreCase)));
```

#### AddTransformation
The `CapitalizeFirstCharacter` and `RemoveMethodPrefixes` are examples of transformation that can be applied to the argument name. You can add your own transformations using `AddTransformation` method.

> [!IMPORTANT]
> All transformations, including `CapitalizeFirstCharacter` and `RemoveMethodPrefixes`, are applied in the order they were added.

#### WithFallback
It is obvious that `RoslynNameConverter` cannot extract a valid name from an arbitrary C# expression, just consider an expression like `2 + 2 * 2`. Thus, you can specify a callback that will be used for such cases using `WithFallbackMethod`.

See the list of supported expressions [below](#how-it-works).

#### WithMemoisation
> [!IMPORTANT]
> While Roslyn is pretty fast, it still adds some overhead, so it's **highly recommended to use memoisation** with the `RoslynNameConverter`.
```cs
IsleConfiguration.Configure(builder => builder.WithRoslynNameConverter(opt => opt.WithMemoisation()));
```

### Full Configuration Example
Here is a complete example configuration:
```cs
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithRoslynNameConverter(opt => opt
        .CapitalizeFirstCharacter()
        .RemoveMethodPrefixes("Get")
        .WithMemoisation()
        .WithFallback(ValueNameConverters.SerilogCompatible())));
```

### How It Works
`RoslynNameConverter` uses C# compiler library (named Roslyn) to parse the argument expression into C# Abstract Syntax Tree (AST). Then it traverses the tree to find the member name that produces the actual value, extracts this member name, applies the configured transformations and returns it to the logging framework.

Thus, for example, if you have the code like this
```cs
logger.LogInformation($"The int property is {unchecked((short) value.IntProperty)}")
```
the converter will successfully extract `IntProperty` as the member name.

The following C# contructs are supported:
+ identifiers (e.g. `value`)
+ member access (e.g. `obj.Property`)
+ method invokation (e.g. `Method(a, b)`, `GetValue()`)
+ conditional member access (e.g. `obj?.Property`, `obj?.Method()`)
+ cast expression (e.g. `(short) value`)
+ pre- and post-increment and decrement (e.g. `++a`, `b--`)
+ pointer indirection (e.g. `*a`)
+ not-null assertion (e.g. `a!`)
+ `checked`/`unchecked` expressions is their content is also supported
+ any valid and unambiguous combinations of them

### Code Analysis

As described above `RoslynNameConverter` cannot support an arbitrary C# expression. While there is a [fallback](WithFallback) for unsupported cases, it's better to prevent them at all by finding such expression and adding `.Named("SomeName")` to them.

Another corner case can be seen in the following example:
```cs
logger.LogInformation($"{a.X} {b.X}");
```
In this case the both argument names will be `X`, which is undesirable.

To find and fix such cases ISLE provides one more package containing code analyzers: Isle.Converters.Roslyn.Analyzers.

If you are using Isle.Converters.Roslyn, it is highly recommended that you also install Isle.Converters.Roslyn.Analyzers to **every project** using ISLE for logging.

> [!TIP]
> If you are using [Centralized Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management), the easiest way is to add it a global package reference to your Directory.Packages.props:
> ```xml
> <ItemGroup>
>   <GlobalPackageReference Include="Isle.Converters.Roslyn.Analyzers" Version="2.0.0" />
> </ItemGroup>
> ```
> Otherwise, consider adding a package reference to Directory.Build.props:
> ```xml
> <ItemGroup>
>   <PackageReference Include="Isle.Converters.Roslyn.Analyzers" Version="2.0.0">
>     <PrivateAssets>all</PrivateAssets>
>     <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
>   </PackageReference>
> </ItemGroup>
> ```

The analyzer can produces the following warnings and provides the code fixes:
| Warning | Description | Code Fixes |
| ------- | ----------- | ---------- |
| ISLE4000 | The converter could not extract a valid name from the expression. | Yes: wrap the expression with `(...).Named("")` |
| ISLE4001 | The name is not unique. | No |
| ISLE4002 | The explicit name in `Named()` is not a constant. | No |
| ISLE4003 | The explicit name in `Named()` is not a valid C# identifier. | No |
| ISLE4004 | The explicit name in `Named()` has leading or trailing whitespace characters. | Yes: trim the name. |

In case you use [`RemoveMethodPrefixes`](#removemethodprefixes) in your configuration, it also necessary to let the analyzer know which prefixes are trimmed. In order to do that add the following to your Directory.Build.props (or individual project file):
```xml
<PropertyGroup>
    <!-- IsleRoslynNameConverterRemoveMethodPrefixes accepts a semicolon-separated list of prefixes that must much the ones you've added in the configuration. -->
    <IsleRoslynNameConverterRemoveMethodPrefixes>Get;Is</IsleRoslynNameConverterRemoveMethodPrefixes>

    <!-- Optional: Ordinal is used by default. The value must much the one you have used in the configuration. -->
    <IsleRoslynNameConverterRemoveMethodPrefixesStringComparison>Ordinal</IsleRoslynNameConverterRemoveMethodPrefixesStringComparison>
</PropertyGroup>
```

## Migrating from v1.x to v2.x

ISLE v2.x remains compatible with all your existing logging code using v1.x, however there are some breaking changes in ISLE configuration that you might need to adopt when migrating to v2.x. Please, see the list of the breaking changes below and suggested fixes for them.

### ISLE Configuration is not resettable by default
ISLE v1.x allows you to reset the configuration by calling `IsleConfiguration.Reset()` and then calling `IsleConfiguration.Configure()` with a new configuration.

While this feature might be useful in some rare cases (e.g. the unit tests of ISLE itself), it's not generally needed by most of the library users, and it prevents ISLE to do some additional performance optimizations. Thus, starting from v2.x the configuration is **not resettable by default**, and calling `Reset()` method will throw `InvalidOperationException`.

If you still need to have a resettable configuration, you must explicitly allow it during each call of `Configure`:
```cs
IsleConfiguration.Configure(builder => builder.IsResettable());
```

See also: [Resettable vs Non-Resettable Configuration](#resettable-vs-non-resettable-configuration)

### Integrations with Microsoft.Extensions.Logging and/or Serilog must be explicitly added to configuration
ISLE 1.x provided extension methods `IIsleConfigurationBuilder.ConfigureExtensionsLogging` and `IIsleConfigutionBuilder.ConfigureSerilog` for the corresponding integration packages that allowed you to configure some behavioral aspects of the corresponding integrations. However, calling these methods was optional, so you could skip them if you did not need any additional configuration.

However, due to changes in the configuration system, ISLE 2.x **requires** you to add the corresponding integration to your configuration explicitly:
```cs
// For Isle.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging());
// For Isle.Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog());
```

If you want to apply the additional configuration to these integrations you can pass an optional delegate to these methods. For example:
```cs
// For Isle.Extensions.Logging
IsleConfiguration.Configure(builder => builder.AddExtensionsLogging(opt => opt.EnableMessageTemplateCaching = true));
// For Isle.Serilog
IsleConfiguration.Configure(builder => builder.AddSerilog(opt => opt.EnableMessageTemplateCaching = true));
```

To simplify migration, the methods `IIsleConfigurationBuilder.ConfigureExtensionsLogging` and `IIsleConfigutionBuilder.ConfigureSerilog` still exist and just call `IIsleConfigurationBuilder.AddExtensionsLogging` and `IIsleConfigutionBuilder.AddSerilog` under the hood correspondingly, however they are marked as `[Obsolete]` and will produce a compilation warning.

## SAST Tools

[PVS-Studio](https://pvs-studio.com/en/pvs-studio/?utm_source=github&utm_medium=organic&utm_campaign=open_source) - static analyzer for C, C++, C#, and Java code.

