using Isle.Configuration;

namespace Isle;

/// <summary>
/// Wrapper struct that allows to set a custom name for the logged value.
/// </summary>
public readonly ref struct NamedLogValue
{
    internal NamedLogValue(object? value, string name, ValueRepresentation representation, Flags flags)
    {
        Value = value;
        Name = name;
        Representation = representation;
        _flags = flags;
    }

    /// <summary>
    /// Gets the wrapped value.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the value name.
    /// </summary>
    public string Name { get; }

    internal ValueRepresentation Representation { get; }

    private readonly Flags _flags;

    /// <summary>
    /// Gets the value indicating whether this instance contains a custom explicit name or a name inferred by the compiler.
    /// </summary>
    /// <returns><see langword="true"/> if the this instance contains a custom explicit name; <see langword="false"/> if it contains a name inferred by the compiler.</returns>
    public bool HasExplicitName => (_flags & Flags.ExplicitName) != default;

    [Flags]
    internal enum Flags
    {
        Default = 0,
        ExplicitName = 1
    }
}