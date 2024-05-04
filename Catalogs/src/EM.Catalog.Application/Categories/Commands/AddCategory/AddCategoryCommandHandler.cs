using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Shared.Core;
using MediatR;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AddCategoryCommandHandler(
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

    public async Task<Result> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(command);
        await _writeRepository.AddCategoryAsync(category, cancellationToken);

        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            throw new DomainException(ErrorMessage.CategoryAnErrorOccorred);
        }

        CategoryAddedEvent categoryAddedEvent = _mapper.Map<CategoryAddedEvent>(category);
        await _mediator.Publish(categoryAddedEvent, cancellationToken);

        return Result.CreateResponseWithData(category.Id);
    }
}
