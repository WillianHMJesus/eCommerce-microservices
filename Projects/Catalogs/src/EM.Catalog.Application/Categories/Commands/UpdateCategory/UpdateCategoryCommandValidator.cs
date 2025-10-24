using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IProductRepository _repository;

    public UpdateCategoryCommandValidator(IProductRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .GreaterThan(Guid.Empty)
            .WithMessage(Category.CategoryInvalidId);

        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(Category.CodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Category.NameNullOrEmpty);

        RuleFor(x => x.Description)
           .Must(x => !string.IsNullOrEmpty(x))
           .WithMessage(Category.DescriptionNullOrEmpty);

        RuleFor(x => x.Id)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryRegistrationAsync(value, cancellationToken))
            .WithMessage(Category.CategoryNotFound);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryHasAlreadyRegisteredAsync(value, cancellationToken))
            .WithMessage(Category.CategoryHasAlreadyBeenRegistered);
    }

    private async Task<bool> ValidateCategoryRegistrationAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }

    private async Task<bool> ValidateCategoryHasAlreadyRegisteredAsync(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetCategoriesByCodeOrName(command.Code, command.Name, cancellationToken);

        return !categories.Any(x => x.Id != command.Id);
    }
}
