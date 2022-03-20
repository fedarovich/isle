using Isle.Configuration;

namespace Isle;

public readonly ref struct NamedLogValue
{
    internal NamedLogValue(object? value, string name, ValueRepresentation representation, Flags flags)
    {
        Value = value;
        Name = name;
        Representation = representation;
        _flags = flags;
    }

    public object? Value { get; }

    public string Name { get; }

    internal ValueRepresentation Representation { get; }

    private readonly Flags _flags;

    public bool HasExplicitName => (_flags & Flags.ExplicitName) != default;

    [Flags]
    internal enum Flags
    {
        Default = 0,
        ExplicitName = 1
    }
}