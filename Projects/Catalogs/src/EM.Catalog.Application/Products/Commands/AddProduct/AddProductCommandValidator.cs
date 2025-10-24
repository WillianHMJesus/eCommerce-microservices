using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    private readonly IProductRepository _repository;

    public AddProductCommandValidator(IProductRepository repository)
	{
        _repository = repository;

		RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Product.NameNullOrEmpty);

        RuleFor(x => x.Description)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Product.DescriptionNullOrEmpty);

        RuleFor(x => x.Value)
            .GreaterThan(default(decimal))
            .WithMessage(Product.ValueLessThanEqualToZero);

        RuleFor(x => x.Quantity)
            .GreaterThan(default(short))
            .WithMessage(Product.QuantityAddedLessThanOrEqualToZero);

        RuleFor(x => x.Image)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Product.ImageNullOrEmpty);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage(Product.InvalidCategoryId);

        RuleFor(x => x.Name)
            .MustAsync(async (_, value, cancellationToken) => await ValidateHasAlreadyBeenRegisteredAsync(value, cancellationToken))
            .WithMessage(Product.ProductHasAlreadyBeenRegistered);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryRegistrationAsync(value, cancellationToken))
            .WithMessage(Category.CategoryNotFound);
    }

    private async Task<bool> ValidateHasAlreadyBeenRegisteredAsync(string name, CancellationToken cancellationToken)
    {
        var products = await _repository.GetByNameAsync(name, cancellationToken);

        return !products.Any();
    }

    private async Task<bool> ValidateCategoryRegistrationAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }
}
