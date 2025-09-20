namespace EM.Authentication.Infraestructure.SettingsOptions;

public sealed record EmailNotificationOptions
{
    public string Smtp { get; set; } = "";
    public int Port { get; set; }
    public CredentialsOptions Credentials { get; set; } = default!;
    public FromOptions From { get; set; } = default!;
}