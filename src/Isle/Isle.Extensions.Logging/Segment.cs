using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Isle.Extensions.Logging;

/// <summary>
/// Segment represents a part of template string.
/// </summary>
internal readonly record struct Segment
{
    /// <summary>
    /// Stores either literal or format.
    /// </summary>
    public string? String { get; }

    /// <summary>
    /// Stores alignment for formatted values.
    /// </summary>
    public int Alignment { get; }

    /// <summary>
    /// Gets the value indicating whether this segments contains a literal.
    /// </summary>
    [MemberNotNullWhen(true, nameof(String))]
    public bool IsLiteral { get; }

    /// <summary>
    /// Creates a literal segment.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Segment(string literal)
    {
        String = literal;
        Alignment = 0;
        IsLiteral = true;
    }

    /// <summary>
    /// Creates a formatted value segment.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Segment(string? format, int alignment)
    {
        String = format;
        Alignment = alignment;
        IsLiteral = false;
    }
}