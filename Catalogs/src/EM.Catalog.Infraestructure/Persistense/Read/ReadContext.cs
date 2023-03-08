using EM.Catalog.Domain.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public class ReadContext
{
    public ReadContext(IOptions<CatalogDatabaseSettings> catalogDatabaseSettings)
    {
        MongoClient client = new(catalogDatabaseSettings.Value.ConnectionString);
        IMongoDatabase database = client.GetDatabase(catalogDatabaseSettings.Value.DatabaseName);

        Products = database.GetCollection<ProductDTO>("Products");
        Categories = database.GetCollection<CategoryDTO>("Categories");
    }

    public IMongoCollection<ProductDTO> Products { get; set; }
    public IMongoCollection<CategoryDTO> Categories { get; set; }
}
