using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.Domain;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(
        IWriteRepository writeRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Product product = _mapper.Map<Product>(command);
        _writeRepository.UpdateProduct(product);
        
        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            throw new DomainException(ErrorMessage.ProductAnErrorOccorred);
        }

        ProductUpdatedEvent productUpdatedEvent = _mapper.Map<ProductUpdatedEvent>(product);
        await _mediator.Publish(productUpdatedEvent, cancellationToken);

        return new Result();
    }
}
