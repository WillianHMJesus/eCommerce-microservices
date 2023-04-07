using EM.Carts.Domain.Entities;

namespace EM.Carts.Domain.Interfaces;

public interface ICartRepository
{
    Task AddCartAsync(Cart cart);
    Task<Cart?> GetCartByUserIdAsync(Guid userId);
    Task UpdateCartAsync(Cart cart);
}
