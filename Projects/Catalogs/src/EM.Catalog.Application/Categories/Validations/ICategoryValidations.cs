using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;

namespace EM.Catalog.Application.Categories.Validations;

public interface ICategoryValidations
{
    Task<bool> ValidateCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<bool> ValidateDuplicityAsync(AddCategoryCommand command, CancellationToken cancellationToken);
    Task<bool> ValidateDuplicityAsync(UpdateCategoryCommand command, CancellationToken cancellationToken);
}
