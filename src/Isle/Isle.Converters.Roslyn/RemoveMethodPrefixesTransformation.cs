namespace Isle.Converters.Roslyn;

internal class RemoveMethodPrefixesTransformation(IEnumerable<string> prefixes, StringComparison stringComparison)
{
    private readonly string[] _prefixes = prefixes?.ToArray() ?? throw new ArgumentNullException(nameof(prefixes));

    internal string Transform(string name, NameExpressionType type)
    {
        if (type == NameExpressionType.Method)
        {
            foreach (var prefix in _prefixes)
            {
                if (name.StartsWith(prefix, stringComparison))
                {
                    return name.Substring(prefix.Length);
                }
            }
        }

        return name;
    }
}
