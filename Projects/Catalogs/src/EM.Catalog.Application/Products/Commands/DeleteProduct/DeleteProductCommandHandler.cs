using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductDeleted;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IResourceManager _resourceManager;

    public DeleteProductCommandHandler(
        IWriteRepository writeRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IResourceManager resourceManager)
    {
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _resourceManager = resourceManager;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _writeRepository.GetProductByIdAsync(request.Id, cancellationToken)
            ?? throw new ArgumentNullException();

        product.Inactivate();
        _writeRepository.DeleteProduct(product);

        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.ProductAnErrorOccorred, cancellationToken);
        }

        ProductDeletedEvent productDeletedEvent = new(request.Id);
        await _mediator.Publish(productDeletedEvent, cancellationToken);

        return new Result();
    }
}
