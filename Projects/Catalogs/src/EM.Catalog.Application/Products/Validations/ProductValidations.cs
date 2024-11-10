using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Validations;

public sealed class ProductValidations : IProductValidations
{
    private readonly IWriteRepository _repository;

    public ProductValidations(IWriteRepository readRepository)
    {
        _repository = readRepository;
    }

    public async Task<bool> ValidateDuplicityAsync(string name, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = 
            await _repository.GetProductsByNameAsync(name, cancellationToken);

        return !products.Any();
    }

    public async Task<bool> ValidateDuplicityAsync(UpdateProductCommand updateProductCommand, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = 
            await _repository.GetProductsByNameAsync(updateProductCommand.Name, cancellationToken);

        return !products.Any(x => x.Id != updateProductCommand.Id);
    }

    public async Task<bool> ValidateProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        Product? product = 
            await _repository.GetProductByIdAsync(productId, cancellationToken);

        return product is not null;
    }
}
