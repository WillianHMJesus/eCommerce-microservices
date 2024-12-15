using EM.Checkout.Application.Models;

namespace EM.Checkout.Application.Interfaces.ExternalServices;

public interface ICartExternalService
{
    Task<CartDTO?> GetItemsByUserId(Guid userId, CancellationToken cancellationToken);
}
