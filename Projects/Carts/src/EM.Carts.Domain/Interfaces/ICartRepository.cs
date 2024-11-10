using EM.Carts.Domain.Entities;

namespace EM.Carts.Domain.Interfaces;

public interface ICartRepository
{
    Task AddCartAsync(Cart cart, CancellationToken cancellationToken);
    Task<Cart?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateCartAsync(Cart cart, CancellationToken cancellationToken);
}
