using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductHandler : ICommandHandler<AddProductCommand>
{
    private readonly IProductRepository _productRepository;

    public AddProductHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Result> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        Category? category = await _productRepository.GetCategoryByIdAsync(command.CategoryId, cancellationToken);

        if (category == null)
            return Result.CreateResponseWithErrors("CategoryId", ErrorMessage.ProductCategoryNotFound);

        Product product = new(command.Name, command.Description, command.Value, command.Quantity, command.Image);
        product.AssignCategory(category);

        await _productRepository.AddProductAsync(product, cancellationToken);

        return Result.CreateResponseWithData(product.Id);
    }
}
