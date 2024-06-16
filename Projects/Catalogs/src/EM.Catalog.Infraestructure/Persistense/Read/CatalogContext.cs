using EM.Catalog.Domain.Entities;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class CatalogContext
{
    private readonly IMongoDatabase _database;

    public CatalogContext(IMongoClient client)
    {
        _database = client.GetDatabase("Catalog");
    }

    public IMongoCollection<Product> Products
        => _database.GetCollection<Product>("Products");
    public IMongoCollection<Category> Categories
        => _database.GetCollection<Category>("Categories");
}
