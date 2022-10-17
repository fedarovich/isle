namespace Isle.Core.Tests;

internal struct TestStruct
{
    public TestStruct(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }
}