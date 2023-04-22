namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed record CatalogDatabaseSettings
{
    public string ConnectionString { get; set; } = ""!;
    public string DatabaseName { get; set; } = ""!;
}