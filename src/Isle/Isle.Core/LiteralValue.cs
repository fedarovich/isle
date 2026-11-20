using Isle.Configuration;

namespace Isle;

/// <summary>
/// Represents a hole value that must be interpreted not as a hole, but as a literal, i.e. the part of message template.
/// </summary>
public readonly ref struct LiteralValue
{
    /// <summary>
    /// Gets the underlying string value.
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
    /// Returns a value that indicates whether this instance and the other <paramref name="literalValue"/>
    /// wrap equal strings and have the same <see cref="IsCacheable"/> value.
    /// </summary>
    /// <param name="literalValue">The literal value to compare with this instance.</param>
    /// <returns>
    /// <see langword="true"/> if both instances have equal <see cref="Value"/> and <see cref="IsCacheable"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(LiteralValue literalValue) => Value == literalValue.Value && IsCacheable == literalValue.IsCacheable;

    /// <summary>
    /// This method is not supported, because <see cref="LiteralValue"/> is a <c>ref struct</c> and cannot be boxed.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    public override bool Equals(object? obj) => throw new NotSupportedException();

    /// <summary>
    /// This method is not supported.
    /// </summary>
    /// <returns>This method never returns.</returns>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    public override int GetHashCode() => throw new NotSupportedException();

    /// <summary>
    /// Returns the wrapped string.
    /// </summary>
    /// <returns>The <see cref="Value"/>, or an empty string if it is <see langword="null"/>.</returns>
    public override string ToString() => Value ?? string.Empty;

    /// <summary>
    /// Converts a string to <see cref="LiteralValue"/>.
    /// </summary>
    /// <param name="value">The string value to wrap.</param>
    /// <remarks>
    /// The cacheability of the result depends on the <see cref="IsleConfiguration.CacheLiteralValues"/> global setting.
    /// </remarks>
    public static explicit operator LiteralValue(string? value) => new (value);

    /// <summary>
    /// Returns a value that indicates whether the two literal values are equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><see langword="true"/> if the values are equal; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Equals(LiteralValue)"/>
    public static bool operator ==(LiteralValue left, LiteralValue right) => left.Equals(right);

    /// <summary>
    /// Returns a value that indicates whether the two literal values are not equal.
    /// </summary>
    /// <param name="left">The first value to compare.</param>
    /// <param name="right">The second value to compare.</param>
    /// <returns><see langword="true"/> if the values are not equal; otherwise, <see langword="false"/>.</returns>
    /// <seealso cref="Equals(LiteralValue)"/>
    public static bool operator !=(LiteralValue left, LiteralValue right) => !left.Equals(right);
}