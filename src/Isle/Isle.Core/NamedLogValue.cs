namespace Isle;

/// <summary>
/// Wrapper struct that allows to set a custom name for the logged value.
/// </summary>
public readonly ref struct NamedLogValue
{
    internal NamedLogValue(object? value, string name)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// Gets the wrapped value.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Gets the value name.
    /// </summary>
    public string Name { get; }
}