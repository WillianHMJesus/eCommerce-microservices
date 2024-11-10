using EM.Catalog.Application.Categories.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ICategoryValidations _validations;

    public UpdateCategoryCommandValidator(ICategoryValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.Id)
            .GreaterThan(Guid.Empty)
            .WithMessage(Key.CategoryInvalidId);

        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(Key.CategoryCodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CategoryNameNullOrEmpty);

        RuleFor(x => x.Description)
           .Must(x => !string.IsNullOrEmpty(x))
           .WithMessage(Key.CategoryDescriptionNullOrEmpty);

        RuleFor(x => x.Id)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateCategoryIdAsync(value, cancellationToken))
            .WithMessage(Key.CategoryNotFound);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(Key.CategoryRegisterDuplicity);
    }
}
