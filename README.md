# ISLE

ISLE (Interpolated String Logging Extensions) is a library that allows developers to perform structured logging using interpolated strings in C# 10 or later.

| Package | Release | Prerelease |
| ------- | ---------- | ------- |
| Isle.Core | [![nuget](https://img.shields.io/nuget/v/Isle.Core.svg?label=nuget)](https://www.nuget.org/packages/Isle.Core) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Core.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Core) |
| Isle.Extensions.Logging | [![nuget](https://img.shields.io/nuget/v/Isle.Extensions.Logging.svg?label=nuget)](https://www.nuget.org/packages/Isle.Extensions.Logging) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Extensions.Logging.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Extensions.Logging) |
| Isle.Serilog | [![nuget](https://img.shields.io/nuget/v/Isle.Serilog.svg?label=nuget)](https://www.nuget.org/packages/Isle.Serilog) | [![myget](https://img.shields.io/myget/fedarovich/vpre/Isle.Serilog.svg?label=myget)](https://www.myget.org/feed/fedarovich/package/nuget/Isle.Serilog) |

[![Build Status](https://dev.azure.com/pavelfedarovich/ISLE/_apis/build/status/fedarovich.isle?branchName=main)](https://dev.azure.com/pavelfedarovich/ISLE/_build/latest?definitionId=12&branchName=main)
[![license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/fedarovich/isle/blob/master/LICENSE)

While originally supported on .Net 6 or later, starting from version 1.5 ISLE has been backported to .Net Standard 2.0 and thus can also be used with .Net Framework 4.6.1 or later, .Net Core 2.0 or later, and .Net 5.

## Getting Started

The current version of ISLE provides a set of extensions methods for:
+ [`Microsoft.Extensions.Logging.ILogger`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-6.0) interface by [Isle.Extensions.Logging](https://www.nuget.org/packages/Isle.Extensions.Logging) package
+ `Serilog.ILogger` interface by [Isle.Serilog](https://www.nuget.org/packages/Isle.Serilog) package. 

In order to perform structured logging you must also add and configure the underlying logging provider (e.g. [Serilog](https://serilog.net)).

To begin using ISLE you must install the corresponding package from NuGet.

> If you are using Serilog only indirectly via `Microsoft.Extensions.Logging.ILogger`, there is no need to install Isle.Serilog. Install just Isle.Extensions.Logging instead.

Next, at the beginning of your program you must configure ISLE. The simplest configuration looks like this:
```
IsleConfiguration.Configure(builder => {});
```
> Configure method must be called before calling any of the ISLE extension methods for the first time.

Now you can simply pass interpolated strings to the ILogger's methods and ISLE will capture the variables names and make the correct template string for the structured logging.

Consider the following example:
```
int width = 5;
int height = 6;
int area = width * height;
logger.LogInformation("The area of rectangle with the width = {width} and the height = {height} is {area}", width, height, area);
```
As you can see, we had to duplicate the names of the variables in the template message string.

ISLE will do it for you:
```
int width = 5;
int height = 6;
int area = width * height;
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {area}");
```
This code with ISLE installed will produce exactly the same structured log message as previous one.

> While Isle.Extensions.Logging is providing the same method names as `Microsoft.Extensions.Logging.ILogger` itself (e.g. `LogError`), the Isle.Serilog's method names have suffix *`Interpolated`* (e.g. `ErrorInterpolated`)

## Value Representation
Any argument you log will be serialized by the logging framework either into a string or a destructured (object) form.

By default, ISLE pass your argument names as is, so that the underlying logging framework selects the representation. For example, Serilog by default uses string presentation for any argument unless its name is preceeded by the **at** sign (`@`). Luckily, you can preceed any C# identifier with `@`.

Consider the following code:
```
public record Point(int X, int Y);

var record = new Record(3, 5);

// Use string form
logger.LogInformation($"The point is {point}.");

// Use destructured form:
logger.LogInformation($"The point is {@point}.");
```

Alternatively, ISLE allows you to specify value representation based on its compile-time type.

For example, to automatically destructure all non-scalar type and collections, we can configure ISLE in the following way:
```
IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring());
```
This code will apply `AutoDestructuringValueRepresentationPolicy` which stringifies the scalar types (e.g. numbers, strings, dates) but destructures custom objects and collections.

You can also create your own custom value representation policy by implementing `IValueRepresentationPolicy` and configuring ISLE to use it:
```
IsleConfiguration.Configure(builder => builder.WithValueRepresentationPolicy(new YourCustomPolicy()));
```

## Custom Argument Names
ISLE automatically captures argument names by using C# 10 CallerArgumentExpressionAttribute. Thus, it is recommended that you always use simple variable or property name as the arguments.

You can also set a custom name for the argument using the `Named` extension method:
```
int width = 5;
int height = 6;
logger.LogInformation($"The area of rectangle with the width = {width} and the height = {height} is {(width * height).Named("area")}");
```

Additionally, you can configure a transform to be applied to the names automatically captured by ISLE. In order to this, create a transformation method and pass it as delegate when configuring ISLE:
```
string CapitalizeFirstLetter(string name) => name[0..1].ToUpper() + name[1..];

IsleConfiguration.Configure(builder => builder.WithNameConverter(CapitalizeFirstLetter));
```
You can find some built-in name converters in the class `Isle.Converters.ValueNameConverters`. It also contains some helper functions for writing your own converters.

Please note that the transform is not applied to the names explicitly specified using the `Named` method.

## SAST Tools

[PVS-Studio](https://pvs-studio.com/en/pvs-studio/?utm_source=github&utm_medium=organic&utm_campaign=open_source) - static analyzer for C, C++, C#, and Java code.

