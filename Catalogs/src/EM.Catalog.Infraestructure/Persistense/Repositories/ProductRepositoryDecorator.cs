using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.EventsReadDatabase.CategoryAdded;
using EM.Catalog.Infraestructure.EventsReadDatabase.CategoryUpdated;
using EM.Catalog.Infraestructure.EventsReadDatabase.ProductAdded;
using EM.Catalog.Infraestructure.EventsReadDatabase.ProductUpdated;
using MediatR;

namespace EM.Catalog.Infraestructure.Persistense.Repositories;

public sealed class ProductRepositoryDecorator : IProductRepository
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


    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productRepository.AddProductAsync(product, cancellationToken);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((ProductAddedEvent)product, cancellationToken);
        }
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productRepository.UpdateProductAsync(product, cancellationToken);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((ProductUpdatedEvent)product, cancellationToken);
        }
    }


    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _productRepository.AddCategoryAsync(category, cancellationToken);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((CategoryAddedEvent)category, cancellationToken);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _productRepository.GetCategoryByIdAsync(id, cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _productRepository.UpdateCategoryAsync(category, cancellationToken);

        if (await _unitOfWork.CommitAsync())
        {
            await _mediator.Publish((CategoryUpdatedEvent)category, cancellationToken);
        }
    }
}
