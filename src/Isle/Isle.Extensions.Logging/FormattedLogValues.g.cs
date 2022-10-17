#nullable enable
#if NETCOREAPP || NETSTANDARD2_1
using System.Runtime.InteropServices;

namespace Isle.Extensions.Logging;


internal sealed class FormattedLogValues0 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues0() : base(1)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.OriginalFormat, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 1 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues1 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues1() : base(2)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 2 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues2 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues2() : base(3)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 3 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.Value1;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.Value1;
            if (Count == 2) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> Value1;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues3 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues3() : base(4)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 4 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.Value1;
            yield return _values.Value2;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.Value1;
            if (Count == 2) yield break;
            yield return _values.Value2;
            if (Count == 3) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> Value1;
        public KeyValuePair<string, object?> Value2;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues4 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues4() : base(5)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 5 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.Value1;
            yield return _values.Value2;
            yield return _values.Value3;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.Value1;
            if (Count == 2) yield break;
            yield return _values.Value2;
            if (Count == 3) yield break;
            yield return _values.Value3;
            if (Count == 4) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> Value1;
        public KeyValuePair<string, object?> Value2;
        public KeyValuePair<string, object?> Value3;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues5 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues5() : base(6)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 6 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.Value1;
            yield return _values.Value2;
            yield return _values.Value3;
            yield return _values.Value4;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.Value1;
            if (Count == 2) yield break;
            yield return _values.Value2;
            if (Count == 3) yield break;
            yield return _values.Value3;
            if (Count == 4) yield break;
            yield return _values.Value4;
            if (Count == 5) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> Value1;
        public KeyValuePair<string, object?> Value2;
        public KeyValuePair<string, object?> Value3;
        public KeyValuePair<string, object?> Value4;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues6 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues6() : base(7)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 7 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.Value1;
            yield return _values.Value2;
            yield return _values.Value3;
            yield return _values.Value4;
            yield return _values.Value5;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.Value1;
            if (Count == 2) yield break;
            yield return _values.Value2;
            if (Count == 3) yield break;
            yield return _values.Value3;
            if (Count == 4) yield break;
            yield return _values.Value4;
            if (Count == 5) yield break;
            yield return _values.Value5;
            if (Count == 6) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> Value1;
        public KeyValuePair<string, object?> Value2;
        public KeyValuePair<string, object?> Value3;
        public KeyValuePair<string, object?> Value4;
        public KeyValuePair<string, object?> Value5;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


internal sealed class FormattedLogValues7 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal FormattedLogValues7() : base(8)
    {
    }

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, Count);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return Count == 8 ? GetEnumeratorFast() : GetEnumeratorSlow();

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorFast()
        {
            yield return _values.Value0;
            yield return _values.Value1;
            yield return _values.Value2;
            yield return _values.Value3;
            yield return _values.Value4;
            yield return _values.Value5;
            yield return _values.Value6;
            yield return _values.OriginalFormat;
        }

        IEnumerator<KeyValuePair<string, object?>> GetEnumeratorSlow()
        {
            if (Count == 0) yield break;
            yield return _values.Value0;
            if (Count == 1) yield break;
            yield return _values.Value1;
            if (Count == 2) yield break;
            yield return _values.Value2;
            if (Count == 3) yield break;
            yield return _values.Value3;
            if (Count == 4) yield break;
            yield return _values.Value4;
            if (Count == 5) yield break;
            yield return _values.Value5;
            if (Count == 6) yield break;
            yield return _values.Value6;
            if (Count == 7) yield break;
            yield return _values.OriginalFormat;
        }
    }

    private struct ValuesPlaceholder
    {
#pragma warning disable CS0649
        public KeyValuePair<string, object?> Value0;
        public KeyValuePair<string, object?> Value1;
        public KeyValuePair<string, object?> Value2;
        public KeyValuePair<string, object?> Value3;
        public KeyValuePair<string, object?> Value4;
        public KeyValuePair<string, object?> Value5;
        public KeyValuePair<string, object?> Value6;
        public KeyValuePair<string, object?> OriginalFormat;
#pragma warning restore CS0649
    }
}


#endif