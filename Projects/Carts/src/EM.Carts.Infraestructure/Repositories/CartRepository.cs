using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.Infraestructure.Configurations;
using MongoDB.Driver;

namespace EM.Carts.Infraestructure.Repositories;

public sealed class CartRepository : ICartRepository
{
    private readonly MongoDbConfiguration _mongoDb;

    public CartRepository(MongoDbConfiguration mongoDb)
        => _mongoDb = mongoDb;

    public async Task AddCartAsync(Cart cart)
        => await _mongoDb.CartCollection.InsertOneAsync(cart);

    public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        => await _mongoDb.CartCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();

    public async Task UpdateCartAsync(Cart cart)
        => await _mongoDb.CartCollection.ReplaceOneAsync(x => x.Id == cart.Id, cart);
}
