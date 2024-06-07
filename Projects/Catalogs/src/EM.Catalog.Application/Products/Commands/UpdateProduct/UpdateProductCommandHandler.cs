using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Common.Core.ResourceManagers;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;

    public UpdateProductCommandHandler(
        IWriteRepository writeRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IMapper mapper,
        IResourceManager resourceManager)
    {
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
        _resourceManager = resourceManager;
    }

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = _mapper.Map<Product>(command);
        _writeRepository.UpdateProduct(product);
        
        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.ProductAnErrorOccorred, cancellationToken);
        }

        ProductUpdatedEvent productUpdatedEvent = _mapper.Map<ProductUpdatedEvent>(product);
        await _mediator.Publish(productUpdatedEvent, cancellationToken);

        return new Result();
    }
}
