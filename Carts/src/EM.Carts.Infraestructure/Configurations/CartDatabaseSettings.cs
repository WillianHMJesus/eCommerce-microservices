namespace EM.Carts.Infraestructure.Configurations;

public sealed record CartDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
