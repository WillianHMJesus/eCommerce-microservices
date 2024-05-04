using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Shared.Core;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandHandler : ICommandHandler<AddProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AddProductCommandHandler(
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

    public async Task<Result> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        Product product = _mapper.Map<Product>(command);
        await _writeRepository.AddProductAsync(product, cancellationToken);
        
        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            throw new DomainException(ErrorMessage.ProductAnErrorOccorred);
        }

        ProductAddedEvent productAddedEvent = _mapper.Map<ProductAddedEvent>(product);
        await _mediator.Publish(productAddedEvent, cancellationToken);

        return Result.CreateResponseWithData(product.Id);
    }
}
