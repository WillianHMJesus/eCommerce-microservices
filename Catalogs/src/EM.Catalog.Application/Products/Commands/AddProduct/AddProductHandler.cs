using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductHandler : IRequestHandler<AddProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public AddProductHandler(IProductRepository productRepository)
        => _productRepository = productRepository; 

    public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        Product product = new(request.Name, request.Description, request.Value, request.Quantity, request.Image);
        await _productRepository.AddAsync(product);

        return product.Id;
    }
}
