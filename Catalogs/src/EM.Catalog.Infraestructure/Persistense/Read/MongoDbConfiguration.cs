using EM.Catalog.Application.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class MongoDbConfiguration
{
    private readonly IMongoDatabase _database;

    public MongoDbConfiguration(IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
    {
        MongoClient client = new(catalogDatabaseSettings.Value.ConnectionString);
        _database = client.GetDatabase(catalogDatabaseSettings.Value.DatabaseName);
    }

    public IMongoCollection<ProductDTO> Products => 
        _database.GetCollection<ProductDTO>("Products");

    public IMongoCollection<CategoryDTO> Categories =>
        _database.GetCollection<CategoryDTO>("Categories");
}
