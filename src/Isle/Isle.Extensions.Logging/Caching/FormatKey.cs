namespace Isle.Extensions.Logging.Caching;

internal readonly record struct FormatKey(Type Type, string RawName, string? Format, int Alignment, bool HasExplicitName);