# Value Representation

A structured logging framework can record a value in one of these forms:

- **default:** primitives (numbers, strings, booleans) are stored as they are. Other values are usually converted to strings, unless the framework has a destructuring rule for their type;
- **destructured:** the value is stored as an object with its public properties, or as a list for collections;
- **stringified:** the value is always converted to a string with `ToString()`.

Message template syntax uses an operator before the property name to ask for a form:

| Operator | Meaning | Example |
| -------- | ------- | ------- |
| *(none)* | let the framework decide | `{point}` |
| `@` | destructure | `{@point}` |
| `$` | stringify | `{$point}` |

> [!NOTE]
> ISLE doesn't destructure or stringify values itself. It only writes the operator into the message template, and the logging framework does the rest. Serilog supports both operators, directly and through `Microsoft.Extensions.Logging`. With other `Microsoft.Extensions.Logging` providers, check whether they understand the operators. A provider that doesn't will see property names such as `@point`.

## The `@` operator in C# code

In C#, you can put `@` in front of any identifier: `@point` is the same variable as `point`. ISLE captures the expression text as written, so the operator gets into the template:

```csharp
public record Point(int X, int Y);

var point = new Point(3, 5);

// Default representation
logger.LogInformation($"The point is {point}.");   // {point}

// Destructured
logger.LogInformation($"The point is {@point}.");  // {@point}
```

This works only on a plain identifier. For other expressions, and for stringification (`$` can't appear in a C# identifier), use [`Named`](argument-names.md#explicit-names-with-named):

```csharp
logger.LogInformation($"Customer {order.Customer.Named("@Customer")}");
logger.LogInformation($"Amount {amount.Named("$Amount")}");
```

> [!NOTE]
> The [Roslyn name converter](roslyn-name-converter.md) keeps the `@` only on a plain identifier (`@point`). For `@order.Customer`, the captured name becomes `Customer` and the `@` is lost, so use `Named` there.

## Value representation policies

A *value representation policy* adds the operators for you, based on the **compile-time** type of each value. It is applied to every captured name that doesn't already start with `@` or `$`. It is also applied to [explicit names](argument-names.md#explicit-names-and-value-representation), unless they preserve the default representation.

The policy implements <xref:Isle.Configuration.IValueRepresentationPolicy>:

```csharp
public interface IValueRepresentationPolicy
{
    ValueRepresentation GetRepresentationOfType<T>();
}
```

It returns one of the <xref:Isle.Configuration.ValueRepresentation> values: `Default`, `Destructure` (adds `@`) or `Stringify` (adds `$`).

### Built-in policies

| Policy | Behavior |
| ------ | -------- |
| <xref:Isle.Configuration.DefaultValueRepresentationPolicy> | Always returns `Default`. This is the policy used when none is configured. |
| <xref:Isle.Configuration.AutoDestructuringValueRepresentationPolicy> | Returns `Default` for scalar types and `Destructure` for everything else. |

The automatic destructuring policy treats these types as scalar. For `Nullable<T>`, it checks the underlying type `T`.

- all primitive types: `bool`, `char`, `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `nint`, `nuint`, `float`, `double`;
- `decimal`, `string`, `Guid`, `Uri`;
- `DateTime`, `DateTimeOffset`, `TimeSpan`, and on .NET 6+ `DateOnly` and `TimeOnly`;
- on .NET 7+, `Int128` and `UInt128`;
- all enum types.

Turn it on with `WithAutomaticDestructuring`:

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithAutomaticDestructuring());
```

> [!CAUTION]
> Destructured values can be large and repetitive. They can also contain data that must not be logged for security or compliance reasons. It is usually better to leave automatic destructuring off and either add `@` where needed, or tell your logging framework (for example, with Serilog's `Destructure` configuration) which types to destructure and how.

> [!NOTE]
> The policy sees the **compile-time** type. A value of a scalar type held in a variable of type `object` is destructured by `AutoDestructuringValueRepresentationPolicy`.

### Custom policies

Implement `IValueRepresentationPolicy` and register it with `WithValueRepresentationPolicy`:

```csharp
using Isle.Configuration;

public sealed class MyPolicy : IValueRepresentationPolicy
{
    public ValueRepresentation GetRepresentationOfType<T>() => Cache<T>.Representation;

    // Computed once per type: the policy is called for every hole of every log call.
    private static class Cache<T>
    {
        public static readonly ValueRepresentation Representation = Compute(typeof(T));
    }

    private static ValueRepresentation Compute(Type type)
    {
        if (type == typeof(Money))
            return ValueRepresentation.Stringify;

        if (typeof(IDomainEvent).IsAssignableFrom(type))
            return ValueRepresentation.Destructure;

        return ValueRepresentation.Default;
    }
}
```

```csharp
IsleConfiguration.Configure(builder => builder
    .AddExtensionsLogging()
    .WithValueRepresentationPolicy(new MyPolicy()));
```

The policy is called for every hole of every log call with an enabled level, from many threads at once. Keep it fast and thread-safe; the generic static cache above is a good pattern.

## How the operator reaches the framework

- **Microsoft.Extensions.Logging:** the operator stays in the key of the state entry (for example `@point`), and the provider decides what to do with it. Serilog's provider destructures or stringifies the value accordingly.
- **Serilog:** ISLE removes the operator from the property name (`point`) and records it on the template token. Serilog then binds the value with destructuring or stringification, as it would for a parsed template.
