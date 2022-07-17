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
    /// Initializes a new instance of <see cref="LiteralValue"/>.
    /// </summary>
    /// <param name="value">The string value to wrap.</param>
    public LiteralValue(string? value) => Value = value;

    /// <summary>
    /// Returns a value that indicates whether this instance wraps the string equal to a the value wrapped with the other <paramref name="literalValue"/>.
    /// </summary>
    public bool Equals(LiteralValue literalValue) => Value == literalValue.Value;

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