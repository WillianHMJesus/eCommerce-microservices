using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Category? category = await _productRepository.GetCategoryByIdAsync(command.CategoryId, cancellationToken);

        if (category == null)
            return Result.CreateResponseWithErrors("CategoryId", ErrorMessage.ProductCategoryNotFound);

        Product product = new(command.Id, command.Name, command.Description, command.Value, command.Quantity, command.Image, command.Available);
        product.AssignCategory(category);

        await _productRepository.UpdateProductAsync(product, cancellationToken);

        return new Result();
    }
}
