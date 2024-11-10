using EM.Carts.Application.DTOs;

namespace EM.Carts.Application.Interfaces.ExternalServices;

public interface ICatalogExternalService
{
    Task<ProductDTO?> GetProductsByIdAsync(Guid id, CancellationToken cancellationToken);
}
