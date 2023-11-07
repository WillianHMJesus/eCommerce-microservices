using EM.Checkout.Application.DTOs;

namespace EM.Checkout.Application.Interfaces;

public interface ICartExternalService
{
    Task<List<ItemDTO>> GetItemsByUserId(Guid userId, CancellationToken cancellationToken);
}
