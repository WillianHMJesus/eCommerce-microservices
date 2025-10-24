using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using FluentValidation;

namespace EM.Catalog.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    private readonly IProductRepository _repository;

    public AddCategoryCommandValidator(IProductRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Code)
            .GreaterThan(default(short))
            .WithMessage(Category.CodeLessThanEqualToZero);

        RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Category.NameNullOrEmpty);

        RuleFor(x => x.Description)
           .Must(x => !string.IsNullOrEmpty(x))
           .WithMessage(Category.DescriptionNullOrEmpty);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryHasAlreadyRegisteredAsync(value.Code, value.Name, cancellationToken))
            .WithMessage(Category.CategoryHasAlreadyBeenRegistered);
    }

    private async Task<bool> ValidateCategoryHasAlreadyRegisteredAsync(short code, string name, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetCategoriesByCodeOrName(code, name, cancellationToken);

        return !categories.Any();
    }
}
