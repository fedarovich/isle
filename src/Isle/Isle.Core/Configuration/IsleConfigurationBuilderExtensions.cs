namespace Isle.Configuration;

/// <summary>
/// Provides extension methods for <see cref="IIsleConfigurationBuilder"/>.
/// </summary>
public static class IsleConfigurationBuilderExtensions
{
    /// <summary>
    /// Assigns the <see cref="AutoDestructuringValueRepresentationPolicy"/> to the <see cref="IIsleConfigurationBuilder.ValueRepresentationPolicy"/> property. 
    /// </summary>
    public static IIsleConfigurationBuilder WithAutomaticDestructuring(this IIsleConfigurationBuilder builder)
    {
        builder.ValueRepresentationPolicy = AutoDestructuringValueRepresentationPolicy.Instance;
        return builder;
    }

    /// <summary>
    /// Assigns the <paramref name="policy"/> to the <see cref="IIsleConfigurationBuilder.ValueRepresentationPolicy"/> property. 
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="policy"/> is <see langword="null"/>.</exception>
    public static IIsleConfigurationBuilder WithValueRepresentationPolicy(this IIsleConfigurationBuilder builder, IValueRepresentationPolicy policy)
    {
        builder.ValueRepresentationPolicy = policy ?? throw new ArgumentNullException(nameof(policy));
        return builder;
    }

    /// <summary>
    /// Assigns the <paramref name="nameConverter"/> to the <see cref="IIsleConfigurationBuilder.ValueNameConverter"/> property.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="nameConverter"/> is <see langword="null"/>.</exception>
    public static IIsleConfigurationBuilder WithNameConverter(this IIsleConfigurationBuilder builder, Func<string, string> nameConverter)
    {
        builder.ValueNameConverter = nameConverter ?? throw new ArgumentNullException(nameof(nameConverter));
        return builder;
    }

    /// <summary>
    /// Assigns the <paramref name="isResettable"/> value to the <see cref="IIsleConfigurationBuilder.IsResettable"/> property.
    /// </summary>
    public static IIsleConfigurationBuilder IsResettable(this IIsleConfigurationBuilder builder, bool isResettable = true)
    {
        builder.IsResettable = isResettable;
        return builder;
    }
}