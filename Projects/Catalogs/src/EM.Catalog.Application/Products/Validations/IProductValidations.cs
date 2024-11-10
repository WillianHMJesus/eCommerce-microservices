using EM.Catalog.Application.Products.Commands.UpdateProduct;

namespace EM.Catalog.Application.Products.Validations;

public interface IProductValidations
{
    Task<bool> ValidateDuplicityAsync(string name, CancellationToken cancellationToken);
    Task<bool> ValidateDuplicityAsync(UpdateProductCommand updateProductCommand, CancellationToken cancellationToken);
    Task<bool> ValidateProductIdAsync(Guid productId, CancellationToken cancellationToken);
}
