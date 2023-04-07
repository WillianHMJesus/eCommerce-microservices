using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using static EM.Catalog.Domain.Entities.Product;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Category? category = await _productRepository.GetCategoryByIdAsync(command.CategoryId);

        if (category == null)
            return Result.CreateResponseWithErrors("CategoryId", ErrorMessage.ProductCategoryNotFound);

        Product product = ProductFactory.NewProduct(command.Id, command.Name, command.Description, command.Value, command.Quantity, command.Image, command.Available);
        product.AssignCategory(category);

        await _productRepository.UpdateProductAsync(product);

        return new Result();
    }
}
