using EM.Catalog.Application.Abstractions;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using WH.SharedKernel;
using WH.SharedKernel.Abstractions;
using WH.SharedKernel.Mediator;
using WH.SharedKernel.ResourceManagers;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Categories.Commands;

public sealed class CategoryCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IMapper mapper) :
    CommandBase(unitOfWork, mediator),
    ICommandHandler<AddCategoryCommand>,
    ICommandHandler<DeleteCategoryCommand>,
    ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = mapper.Map<AddCategoryCommand, Category>(command);
        await repository.AddCategoryAsync(category, cancellationToken);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => mapper.Map<Category, CategoryAddedEvent>(category),
            Category.ErrorSavingCategory,
            Result.CreateResponseWithData(category.Id)
        );
    }

    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await repository.GetCategoryByIdAsync(command.Id, cancellationToken);
            
        if (category is null)
        {
            return Result.CreateResponseWithErrors([new Error("ApplicationError", Category.CategoryNotFound)]);
        }

        repository.DeleteCategory(category);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => new CategoryDeletedEvent() { Id = category.Id },
            Category.ErrorSavingCategory
        );
    }

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = mapper.Map<UpdateCategoryCommand, Category>(command);
        repository.UpdateCategory(category);

        return await CommitAndPublishAsync(
            cancellationToken,
            () => mapper.Map<Category, CategoryUpdatedEvent>(category),
            Category.ErrorSavingCategory
        );
    }
}
