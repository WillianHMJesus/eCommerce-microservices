using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    private readonly IReadRepository _repository;

    public AddProductCommandValidator(IReadRepository repository)
	{
        _repository = repository;

		RuleFor(x => x.Name)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.ProductNameNullOrEmpty);

        RuleFor(x => x.Description)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.ProductDescriptionNullOrEmpty);

        RuleFor(x => x.Value)
            .GreaterThan(default(decimal))
            .WithMessage(Key.ProductValueLessThanEqualToZero);

        RuleFor(x => x.Quantity)
            .GreaterThan(default(short))
            .WithMessage(Key.ProductQuantityLessThanEqualToZero);

        RuleFor(x => x.Image)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.ProductImageNullOrEmpty);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage(Key.ProductInvalidCategoryId);

        RuleFor(x => x.Name)
            .MustAsync(async (_, value, cancellationToken) => await ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(Key.ProductRegisterDuplicity);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryIdAsync(value, cancellationToken))
            .WithMessage(Key.ProductCategoryNotFound);
    }

    public async Task<bool> ValidateDuplicityAsync(string name, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = await _repository.GetProductsByNameAsync(name, cancellationToken);

        return !products.Any();
    }

    public async Task<bool> ValidateCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        Category? category = await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }
}
