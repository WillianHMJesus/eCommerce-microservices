namespace EM.Carts.Application.Validations;

public interface IGenericValidations
{
    Task<bool> ValidateCartByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> ValidateItemByProductIdAsync(Guid productId, Guid userId, CancellationToken cancellationToken);
    Task<bool> ValidateProductAvailabilityAsync(Guid productId, CancellationToken cancellationToken);
}
