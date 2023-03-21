using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;

namespace EM.Catalog.Infraestructure.Persistense.Decorators;

public class ProductRepositoryDecorator : IProductRepository
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public ProductRepositoryDecorator(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task AddProductAsync(Product product)
    {
        await _productRepository.AddProductAsync(product);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((ProductAddedEvent)product);
        }
    }

    public async Task AddProductAsync(ProductDTO product)
    {
        await _productRepository.AddProductAsync(product);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(short page = 1, short pageSize = 10)
    {
        return await _productRepository.GetAllProductsAsync(page, pageSize);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id)
    {
        return await _productRepository.GetProductByIdAsync(id);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(Guid categoryId)
    {
        return await _productRepository.GetProductsByCategoryIdAsync(categoryId);
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateProductAsync(product);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((ProductUpdatedEvent)product);
        }
    }

    public async Task UpdateProductAsync(ProductDTO product)
    {
        await _productRepository.UpdateProductAsync(product);
    }
}
