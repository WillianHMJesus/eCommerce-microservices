namespace Em.Common.Infraestructure.DatabaseSettings;

public sealed record MessageDatabaseSettings
{
    public string ConnectionString { get; set; } = ""!;
    public string DatabaseName { get; set; } = ""!;
    public string ErrorsCollectionName { get; set; } = ""!;
}
