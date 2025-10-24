using EM.Catalog.Domain.Entities;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Category.CategoryInvalidId);
    }
}
