using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Isle.Extensions.Logging;

/// <summary>
/// Segment represents a part of template string.
/// </summary>
/// <remarks>
/// This struct is very low level and does not provide good encapsulation for performance reasons.
/// Thus we don't want to make it public.
/// </remarks>
internal readonly record struct Segment
{
    internal enum SegmentType
    {
        /// <summary>
        /// The segment stores a formatted value alignment and format.
        /// </summary>
        FormattedValue = -1,

        /// <summary>
        /// The segment stores a literal value.
        /// </summary>
        Literal = 0,

        /// <summary>
        /// The segment stores a list of sequential literal values (without formatted values between them).
        /// </summary>
        LiteralList = 1
    }

    /// <summary>
    /// Stores either a string (for literal or format) or <see cref="List{String}"/> containing sequential literals.
    /// </summary>
    private readonly object? _object;

    /// <summary>
    /// Stores alignment for formatted values, 0 for literals and index of the first literal in the list for literal list.
    /// </summary>
    private readonly int _alignmentOrStart;

    /// <summary>
    /// Stores -1 for formatted values, 0 for literals or the number of literals in the literal lists.
    /// </summary>
    private readonly int _count;

    /// <summary>
    /// Gets the segment type.
    /// </summary>
    public SegmentType Type => (SegmentType) Math.Sign(_count);

    /// <summary>
    /// Creates a literal segment.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Segment(string literal)
    {
        _object = literal;
        _alignmentOrStart = 0;
        _count = 0;
    }

    /// <summary>
    /// Creates a formatted value segment.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Segment(string? format, int alignment)
    {
        _object = format;
        _alignmentOrStart = alignment;
        _count = -1;
    }

    /// <summary>
    /// Creates a literal list segment by merging an existing literal segment and a literal,
    /// and putting their content in the end of <paramref name="literalList"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Segment(in Segment mergeWith, string literal, List<string> literalList)
    {
        Debug.Assert(mergeWith.Type == SegmentType.Literal);

        int start = literalList.Count;
        literalList.Add(mergeWith.Literal);
        literalList.Add(literal);
        _object = literalList;
        _alignmentOrStart = start;
        _count = 2;
    }

    /// <summary>
    /// Creates a literal list segment by merging an existing literal list segment and a literal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Segment(in Segment mergeWith, string literal)
    {
        Debug.Assert(mergeWith.Type == SegmentType.LiteralList);

        var list = (List<string>) mergeWith._object!;
        list.Add(literal);
        _object = list;
        _alignmentOrStart = mergeWith._alignmentOrStart;
        _count = mergeWith._count + 1;
    }

    public string Literal => (_object as string)!;

    public string? Format => _object as string;

    public Span<string> LiteralList => CollectionsMarshal.AsSpan(_object as List<string>).Slice(_alignmentOrStart, _count);

    public int Alignment => _alignmentOrStart;
}