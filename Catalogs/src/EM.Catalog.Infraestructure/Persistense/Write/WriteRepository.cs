using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class WriteRepository : IWriteRepository
{
    private readonly CatalogContext _writeContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public WriteRepository(
        CatalogContext writeContext,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IMapper mapper)
    {
        _writeContext = writeContext;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _writeContext.Products.AddAsync(product, cancellationToken);

        if (await _unitOfWork.CommitAsync())
        {
            ProductAddedEvent productAddedEvent = _mapper.Map<ProductAddedEvent>(product);
            await _mediator.Publish(productAddedEvent, cancellationToken);
        }
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _writeContext.Products.Update(product);

        if (await _unitOfWork.CommitAsync())
        {
            ProductUpdatedEvent productUpdatedEvent = _mapper.Map<ProductUpdatedEvent>(product);
            await _mediator.Publish(productUpdatedEvent, cancellationToken);
        }
    }

    public async Task AddCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        await _writeContext.Categories.AddAsync(category, cancellationToken);

        if (await _unitOfWork.CommitAsync())
        {
            CategoryAddedEvent categoryAddedEvent = _mapper.Map<CategoryAddedEvent>(category);
            await _mediator.Publish(categoryAddedEvent, cancellationToken);
        }
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _writeContext.Categories.FindAsync(id, cancellationToken);
    }

    public async Task UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        _writeContext.Categories.Update(category);

        if (await _unitOfWork.CommitAsync())
        {
            CategoryUpdatedEvent categoryUpdatedEvent = _mapper.Map<CategoryUpdatedEvent>(category);
            await _mediator.Publish(categoryUpdatedEvent, cancellationToken);
        }
    }
}
