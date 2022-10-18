#if !NET5_0_OR_GREATER
#nullable disable
using System.Diagnostics;

namespace Isle.Extensions.Logging;


internal readonly partial record struct Segment
{
    public readonly ref struct ListSegment<T>
    {
        private readonly List<T> _list;
        private readonly int _offset;
        private readonly int _length;

        public ListSegment(List<T> list, int offset, int length)
        {
            Debug.Assert(offset + length <= list.Count);

            _list = list;
            _offset = offset;
            _length = length;
        }

        public T this[int index]
        {
            get => _list[_offset + index];
            set => _list[_offset + index] = value;
        }

        public int Length => _length;

        public ListSegmentEnumerator<T> GetEnumerator() => new(_list, _offset, _length);

        public T[] ToArray() => _list.Skip(_offset).Take(_length).ToArray();
    }

    public ref struct ListSegmentEnumerator<T>
    {
        private readonly List<T> _list;
        private readonly int _end;
        private int _index;
        private T _current;

        public ListSegmentEnumerator(List<T> list, int offset, int length) : this()
        {
            Debug.Assert(offset + length <= list.Count);

            _list = list;
            _index = offset;
            _end = offset + length;
            _current = default;
        }

        public bool MoveNext()
        {
            if (_index < _end)
            {
                _current = _list[_index];
                _index++;
                return true;
            }

            _current = default;
            return false;
        }

        public T Current => _current;
    }
}
#endif