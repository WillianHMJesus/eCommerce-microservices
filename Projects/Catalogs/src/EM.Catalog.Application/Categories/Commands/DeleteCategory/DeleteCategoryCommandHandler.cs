using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IResourceManager _resourceManager;

    public DeleteCategoryCommandHandler(
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

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await _writeRepository.GetCategoryByIdAsync(request.Id, cancellationToken)
            ?? throw new ArgumentNullException();

        category.Inactivate();
        _writeRepository.DeleteCategory(category);

        if (!await _unitOfWork.CommitAsync(cancellationToken))
        {
            return await _resourceManager.GetErrorsByKeyAsync(Key.CategoryAnErrorOccorred, cancellationToken);
        }

        CategoryDeletedEvent categoryDeletedEvent = new(request.Id);
        await _mediator.Publish(categoryDeletedEvent, cancellationToken);

        return new Result();
    }
}
