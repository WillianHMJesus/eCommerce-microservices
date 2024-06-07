using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandHandler : ICommandHandler<AddCategoryCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;

    public AddCategoryCommandHandler(
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

    public async Task<Result> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(command);
        await _writeRepository.AddCategoryAsync(category, cancellationToken);

        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.CategoryAnErrorOccorred, cancellationToken);
        }

        CategoryAddedEvent categoryAddedEvent = _mapper.Map<CategoryAddedEvent>(category);
        await _mediator.Publish(categoryAddedEvent, cancellationToken);

        return Result.CreateResponseWithData(category.Id);
    }
}
