using EM.Catalog.Domain;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(ErrorMessage.CategoryCodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage(ErrorMessage.CategoryNameNullOrEmpty);

        RuleFor(x => x.Description)
           .NotEmpty()
           .NotNull()
           .WithMessage(ErrorMessage.CategoryDescriptionNullOrEmpty);
    }
}
