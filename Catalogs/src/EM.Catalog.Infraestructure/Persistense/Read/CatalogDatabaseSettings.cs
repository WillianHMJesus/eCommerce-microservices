namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed record CatalogDatabaseSettings
{
    public string ConnectionString { get; set; } = ""!;
    public string DatabaseName { get; set; } = ""!;
    public string ProductsCollectionName { get; set; } = ""!;
    public string CategoriesCollectionName { get; set; } = ""!;
}