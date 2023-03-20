using EM.Catalog.Domain;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(Guid.Empty)
            .WithMessage(ErrorMessage.CategoryInvalidId);

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
