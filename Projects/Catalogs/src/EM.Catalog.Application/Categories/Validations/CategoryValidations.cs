using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Validations;

public sealed class CategoryValidations : ICategoryValidations
{
    private readonly IWriteRepository _repository;

    public CategoryValidations(IWriteRepository readRepository)
    {
        _repository = readRepository;
    }

    public async Task<bool> ValidateCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        Category? category =
            await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }

    public async Task<bool> ValidateDuplicityAsync(AddCategoryCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories =
            await _repository.GetCategoriesByCodeOrName(command.Code, command.Name, cancellationToken);

        return !categories.Any();
    }

    public async Task<bool> ValidateDuplicityAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories =
            await _repository.GetCategoriesByCodeOrName(command.Code, command.Name, cancellationToken);

        return !categories.Any(x => x.Id != command.Id);
    }
}
