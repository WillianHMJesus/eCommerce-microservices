using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.MakeUnavailableProduct;

public sealed class MakeUnavailableProductCommandHandler : ICommandHandler<MakeUnavailableProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;

    public MakeUnavailableProductCommandHandler(
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

    public async Task<Result> Handle(MakeUnavailableProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _writeRepository.GetProductByIdAsync(request.Id, cancellationToken)
            ?? throw new ArgumentNullException();

        product.MakeUnavailable();
        _writeRepository.UpdateProductAvailable(product);

        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.ProductAnErrorOccorred, cancellationToken);
        }

        ProductUpdatedEvent productUpdatedEvent = _mapper.Map<ProductUpdatedEvent>(product);
        await _mediator.Publish(productUpdatedEvent, cancellationToken);

        return new Result();
    }
}
