using EM.Catalog.Domain.Entities;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Read;

public sealed class CatalogContext
{
    private readonly IMongoDatabase _database;

    public CatalogContext(IMongoClient client)
    {
        _database = client.GetDatabase("Catalog");

        var ignoreExtraElements = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", ignoreExtraElements, type => true);

        var camelCaseElement = new ConventionPack() { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseElement", camelCaseElement, type => true);
    }

    public IMongoCollection<Product> Products
        => _database.GetCollection<Product>("Products");
    public IMongoCollection<Category> Categories
        => _database.GetCollection<Category>("Categories");
}
