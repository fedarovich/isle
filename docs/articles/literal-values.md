# Literal Values

Sometimes a value in an interpolated string should become part of the message template text, not a property. A common example is `nameof(...)`:

```csharp
logger.LogWarning($"Retrieved the {@value} from the {nameof(LocalStorageConfigSource)}.");
// Template: "Retrieved the {@value} from the {nameof(LocalStorageConfigSource)}."
```

Here `value` should be a property, but `nameof(LocalStorageConfigSource)` is a compile-time constant and belongs in the template text. Wrap it in <xref:Isle.LiteralValue>:

```csharp
using Isle;

logger.LogWarning($"Retrieved the {@value} from the {new LiteralValue(nameof(LocalStorageConfigSource), true)}.");
// Template: "Retrieved the {@value} from the LocalStorageConfigSource."
```

## Creating a `LiteralValue`

| Code | Cacheable? |
| ---- | ---------- |
| `new LiteralValue(text, isCacheable: true)` | yes |
| `new LiteralValue(text, isCacheable: false)` | no |
| `new LiteralValue(text)` | the global [`CacheLiteralValues`](configuration.md#cacheliteralvalues) setting (default `false`) |
| `(LiteralValue)text` | the global `CacheLiteralValues` setting |

Prefer the two-argument constructor, so the caching decision is visible at each call.

Other details:

- A `null` or empty literal is skipped.
- Braces in the literal are escaped in the template, so the text appears as written.
- Alignment and format specifiers (`{literal,10}`) aren't supported for literal values.
- `LiteralValue` is a `ref struct`, so it can only be created inline or held in a local variable.

## Literal values and template caching

When [template caching](configuration.md#template-caching) is on (the default), ISLE caches every distinct template it sees. A **cacheable** literal becomes part of the cached template, so each distinct literal text creates a new cache entry, and the entries are never removed.

A **non-cacheable** literal is handled without the cache. The template that contains it is built again on every call, which is slower but uses no extra memory.

> [!CAUTION]
> Mark a literal as cacheable only if it is:
> - a compile-time or runtime constant, such as `nameof(Something)` or a `const` field; or
> - one of a small, fixed set of values, such as a `bool` or a small enum converted to a string.
>
> Caching literals with unbounded values (user input, IDs, timestamps, ...) causes a **memory leak**.

`LiteralValue` is meant for text that describes the event. If the value changes from call to call, it is usually better to log it as a property.
