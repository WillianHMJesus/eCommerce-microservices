using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using MongoDB.Driver;

namespace EM.Carts.Infraestructure.Persistense;

public sealed class CartRepository : ICartRepository
{
    private readonly CartContext _context;

    public CartRepository(CartContext context)
    {
        _context = context;
    }

    public async Task AddCartAsync(Cart cart, CancellationToken cancellationToken)
    {
        await _context.Carts.InsertOneAsync(cart, cancellationToken: cancellationToken);
    }

    public async Task<Cart?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Carts.Find(x => x.UserId == userId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken)
    {
        await _context.Carts.ReplaceOneAsync(x => x.Id == cart.Id, cart, cancellationToken: cancellationToken);
    }
}
