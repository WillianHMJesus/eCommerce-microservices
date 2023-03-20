using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using static EM.Catalog.Domain.Entities.Category;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = CategoryFactory.NewCategory(command.Id, command.Code, command.Name, command.Description);
        await _categoryRepository.UpdateCategoryAsync(category);

        return new Result();
    }
}
