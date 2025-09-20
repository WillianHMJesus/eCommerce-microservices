namespace EM.Authentication.Infraestructure.SettingsOptions;

public sealed record CredentialsOptions
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
