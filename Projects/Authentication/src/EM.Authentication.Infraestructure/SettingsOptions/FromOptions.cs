namespace EM.Authentication.Infraestructure.SettingsOptions;

public sealed record FromOptions
{
    public string Name { get; set; } = "";
    public string EmailAddress { get; set; } = "";
}
