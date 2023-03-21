using EM.Catalog.Domain.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public class ReadContext
{
    private readonly IMongoDatabase _database;

    public ReadContext(IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
    {
        MongoClient client = new(catalogDatabaseSettings.Value.ConnectionString);

        _database = client.GetDatabase(catalogDatabaseSettings.Value.DatabaseName);
    }

    public IMongoCollection<ProductDTO> Products =>
        _database.GetCollection<ProductDTO>("Products");

    public IMongoCollection<CategoryDTO> Categories =>
        _database.GetCollection<CategoryDTO>("Categories");
}
