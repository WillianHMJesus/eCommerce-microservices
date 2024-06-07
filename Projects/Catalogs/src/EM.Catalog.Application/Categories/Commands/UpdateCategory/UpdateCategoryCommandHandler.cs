using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IResourceManager _resourceManager;

    public UpdateCategoryCommandHandler(
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

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(command);
        _writeRepository.UpdateCategory(category);

        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.CategoryAnErrorOccorred, cancellationToken);
        }

        CategoryUpdatedEvent categoryUpdatedEvent = _mapper.Map<CategoryUpdatedEvent>(category);
        await _mediator.Publish(categoryUpdatedEvent, cancellationToken);

        return new Result();
    }
}
