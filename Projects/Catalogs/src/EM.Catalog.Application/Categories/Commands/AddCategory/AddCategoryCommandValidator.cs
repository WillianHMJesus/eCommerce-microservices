using EM.Catalog.Application.Categories.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    private readonly ICategoryValidations _validations;

    public AddCategoryCommandValidator(ICategoryValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(Key.CategoryCodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CategoryNameNullOrEmpty);

        RuleFor(x => x.Description)
           .Must(x => !string.IsNullOrEmpty(x))
           .WithMessage(Key.CategoryDescriptionNullOrEmpty);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(Key.CategoryRegisterDuplicity);
    }
}
