using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IReadRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(
        IWriteRepository writeRepository,
        IReadRepository readRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IMapper mapper)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        Category? category = await _readRepository.GetCategoryByIdAsync(command.CategoryId, cancellationToken);

        if (category == null)
            return Result.CreateResponseWithErrors("CategoryId", ErrorMessage.ProductCategoryNotFound);

        Product product = _mapper.Map<Product>(command);
        product.AssignCategory(category);

        _writeRepository.UpdateProduct(product);
        if (await _unitOfWork.CommitAsync(cancellationToken))
        {
            ProductUpdatedEvent productUpdatedEvent = _mapper.Map<ProductUpdatedEvent>(product);
            await _mediator.Publish(productUpdatedEvent, cancellationToken);
        }

        return new Result();
    }
}
