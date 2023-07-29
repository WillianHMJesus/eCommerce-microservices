using EM.Carts.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EM.Carts.Infraestructure.Configurations;

public sealed class MongoDbConfiguration
{
    private readonly IMongoDatabase _database;

    public MongoDbConfiguration(IOptions<CartDatabaseSettings> options)
    {
        MongoClient client = new(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<Cart> CartCollection => 
        _database.GetCollection<Cart>("Carts");
}
