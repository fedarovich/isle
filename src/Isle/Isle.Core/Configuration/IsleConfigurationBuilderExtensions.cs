namespace Isle.Configuration;

public static class IsleConfigurationBuilderExtensions
{
    public static IIsleConfigurationBuilder WithAutomaticDestructuring(this IIsleConfigurationBuilder builder)
    {
        builder.ValueRepresentationPolicy = AutoDestructuringValueRepresentationPolicy.Instance;
        return builder;
    }

    public static IIsleConfigurationBuilder WithValueRepresentationPolicy(this IIsleConfigurationBuilder builder, IValueRepresentationPolicy policy)
    {
        builder.ValueRepresentationPolicy = policy ?? throw new ArgumentNullException(nameof(policy));
        return builder;
    }

    public static IIsleConfigurationBuilder WithNameConverter(this IIsleConfigurationBuilder builder, Func<string, string> nameConverter)
    {
        builder.ValueNameConverter = nameConverter ?? throw new ArgumentNullException(nameof(nameConverter));
        return builder;
    }
}