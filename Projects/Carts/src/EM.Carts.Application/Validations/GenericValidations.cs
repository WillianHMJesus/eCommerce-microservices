using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces.ExternalServices;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.Validations;

public sealed class GenericValidations : IGenericValidations
{
    private readonly ICartRepository _repository;
    private readonly ICatalogExternalService _catalogExternalService;

    public GenericValidations(
        ICartRepository repository,
        ICatalogExternalService catalogExternalService)
    {
        _repository = repository;
        _catalogExternalService = catalogExternalService;
    }

    public async Task<bool> ValidateCartByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        Cart? cart = await _repository.GetCartByUserIdAsync(userId, cancellationToken);

        return cart is not null;
    }

    public async Task<bool> ValidateItemByProductIdAsync(Guid productId, Guid userId, CancellationToken cancellationToken)
    {
        Cart? cart = await _repository.GetCartByUserIdAsync(userId, cancellationToken);

        if (cart == null) return false;

        Item? existingItem = cart?.Items.FirstOrDefault(x => x.ProductId == productId);

        return existingItem is not null;
    }

    public async Task<bool> ValidateProductAvailabilityAsync(Guid productId, CancellationToken cancellationToken)
    {
        ProductDTO? product = await _catalogExternalService.GetProductsByIdAsync(productId, cancellationToken);

        if (product == null) return false;

        return product.Available && product.Quantity > 0;
    }
}
