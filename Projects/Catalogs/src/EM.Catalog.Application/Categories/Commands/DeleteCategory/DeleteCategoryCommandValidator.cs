using EM.Catalog.Application.Categories.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly ICategoryValidations _validations;

    public DeleteCategoryCommandValidator(ICategoryValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Key.CategoryInvalidId);

        RuleFor(x => x.Id)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateCategoryIdAsync(value, cancellationToken))
            .WithMessage(Key.CategoryNotFound);
    }
}
