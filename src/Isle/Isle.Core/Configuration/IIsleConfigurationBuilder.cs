namespace Isle.Configuration;

public interface IIsleConfigurationBuilder
{
    IValueRepresentationPolicy? ValueRepresentationPolicy { get; set; }

    Func<string, string>? ValueNameConverter { get; set; }
}