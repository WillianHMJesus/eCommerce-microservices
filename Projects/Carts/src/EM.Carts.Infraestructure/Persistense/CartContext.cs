using EM.Carts.Domain.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace EM.Carts.Infraestructure.Persistense;

public sealed class CartContext
{
    private readonly IMongoDatabase _database;

    public CartContext(IMongoClient client)
    {
        _database = client.GetDatabase("Cart");

        var ignoreExtraElements = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", ignoreExtraElements, type => true);

        var camelCaseElement = new ConventionPack() { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseElement", camelCaseElement, type => true);
    }

    public IMongoCollection<Cart> Carts
        => _database.GetCollection<Cart>("Carts");
}
