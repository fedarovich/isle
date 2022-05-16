#nullable enable
using System.Runtime.InteropServices;

namespace Isle.Extensions.Logging;


internal sealed class FormattedLogValues0 : FormattedLogValuesBase
{
    private ValuesPlaceholder _values;

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.OriginalFormat, 1);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 2);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.Value0;
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 3);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.Value0;
        yield return _values.Value1;
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 4);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.Value0;
        yield return _values.Value1;
        yield return _values.Value2;
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 5);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.Value0;
        yield return _values.Value1;
        yield return _values.Value2;
        yield return _values.Value3;
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 6);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.Value0;
        yield return _values.Value1;
        yield return _values.Value2;
        yield return _values.Value3;
        yield return _values.Value4;
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 7);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        yield return _values.Value0;
        yield return _values.Value1;
        yield return _values.Value2;
        yield return _values.Value3;
        yield return _values.Value4;
        yield return _values.Value5;
        yield return _values.OriginalFormat;
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

    internal override Span<KeyValuePair<string, object?>> Values => MemoryMarshal.CreateSpan(ref _values.Value0, 8);
    
    public override IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
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

