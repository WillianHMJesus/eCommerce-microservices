using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.EventsReadDatabase.CategoryAdded;
using EM.Catalog.Infraestructure.EventsReadDatabase.CategoryUpdated;
using EM.Catalog.Infraestructure.EventsReadDatabase.ProductAdded;
using EM.Catalog.Infraestructure.EventsReadDatabase.ProductUpdated;
using MediatR;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

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

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateProductAsync(product);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((ProductUpdatedEvent)product);
        }
    }


    public async Task AddCategoryAsync(Category category)
    {
        await _productRepository.AddCategoryAsync(category);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((CategoryAddedEvent)category);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _productRepository.GetCategoryByIdAsync(id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        await _productRepository.UpdateCategoryAsync(category);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((CategoryUpdatedEvent)category);
        }
    }
}
