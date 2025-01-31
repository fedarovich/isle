using Isle.Configuration;

namespace Isle;

/// <summary>
/// Represents a hole value that must be interpreted not as a hole, but as a literal, i.e. the part of message template.
/// </summary>
public readonly ref struct LiteralValue
{
    /// <summary>
    /// Get the underlying string value.
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Gets the value indicating whether this literal value can be cached.
    /// </summary>
    public bool IsCacheable { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="LiteralValue"/>.
    /// </summary>
    /// <param name="value">The string value to wrap.</param>
    /// <remarks>
    /// <para>
    /// The cacheability of the literal value depends on <see cref="IsleConfiguration.CacheLiteralValues"/>
    /// global setting if this constructor is used.
    /// </para>
    /// <para>
    /// CAUTION! Literal values may be cached only if they are compile-time or run-time constants.
    /// If literal value caching is enabled, passing non-constant values as <see cref="LiteralValue"/>s
    /// will cause memory leaks.
    /// </para>
    /// </remarks>
    public LiteralValue(string? value) : this(value, CoreConfiguration.CacheLiteralValues)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LiteralValue"/>.
    /// </summary>
    /// <param name="value">The string value to wrap.</param>
    /// <param name="isCacheable">The value indicating whether this literal value can be cached.</param>
    /// <remarks>
    /// CAUTION! Literal values may be cached only if they are compile-time or run-time constants.
    /// If literal value caching is enabled, passing non-constant values as <see cref="LiteralValue"/>s
    /// will cause memory leaks.
    /// </remarks>
    public LiteralValue(string? value, bool isCacheable) => (Value, IsCacheable) = (value, isCacheable);

    /// <summary>
    /// Returns a value that indicates whether this instance wraps the string equal to a the value wrapped with the other <paramref name="literalValue"/>.
    /// </summary>
    public bool Equals(LiteralValue literalValue) => Value == literalValue.Value && IsCacheable == literalValue.IsCacheable;

    /// <inheritdoc />
    public override bool Equals(object? obj) => throw new NotSupportedException();

    /// <inheritdoc />
    public override int GetHashCode() => throw new NotSupportedException();

    /// <inheritdoc />
    public override string ToString() => Value ?? string.Empty;

    /// <summary>
    /// Converts a string to <see cref="LiteralValue"/>.
    /// </summary>
    public static explicit operator LiteralValue(string? value) => new (value);

    /// <summary>
    /// Returns a value that indicates whether the two string literals wrap equal strings.
    /// </summary>
    public static bool operator ==(LiteralValue left, LiteralValue right) => left.Equals(right);

    /// <summary>
    /// Returns a value that indicates whether the two string literals wrap not equal strings.
    /// </summary>
    public static bool operator !=(LiteralValue left, LiteralValue right) => !left.Equals(right);
}