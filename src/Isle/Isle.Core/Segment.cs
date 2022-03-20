using System.Diagnostics;

namespace Isle;

internal readonly record struct Segment
{
    private readonly int _startOrAlignment;
    private readonly int _length;

    public Segment(int alignment)
    {
        _startOrAlignment = alignment;
        _length = -1;
    }

    public Segment(int start, int length)
    {
        Debug.Assert(length >= 0);
        _startOrAlignment = start;
        _length = length;
    }

    public int Start
    {
        get => _startOrAlignment;
        init => _startOrAlignment = value;
    }

    public int Alignment
    {
        get => _startOrAlignment;
        init => _startOrAlignment = value;
    }

    public int Length
    {
        get => _length;
        init => _length = value;
    }

    public bool IsLiteral => _length >= 0;

    public static void Grow(ref Segment segment, int lengthIncrement)
    {
        Debug.Assert(segment.IsLiteral);
        segment = segment with { Length = segment.Length + lengthIncrement };
    }
}