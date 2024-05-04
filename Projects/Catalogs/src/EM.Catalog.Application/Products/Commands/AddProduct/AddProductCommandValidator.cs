using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
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
            .WithMessage(ErrorMessage.ProductNameNullOrEmpty);

        RuleFor(x => x.Description)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(ErrorMessage.ProductDescriptionNullOrEmpty);

        RuleFor(x => x.Value)
            .GreaterThan(default(decimal))
            .WithMessage(ErrorMessage.ProductValueLessThanEqualToZero);

        RuleFor(x => x.Quantity)
            .GreaterThan(default(short))
            .WithMessage(ErrorMessage.ProductQuantityLessThanEqualToZero);

        RuleFor(x => x.Image)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(ErrorMessage.ProductImageNullOrEmpty);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage(ErrorMessage.ProductInvalidCategoryId);

        RuleFor(x => x.Name)
            .MustAsync(async (_, value, cancellationToken) => await ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(ErrorMessage.ProductRegisterDuplicity);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryIdAsync(value, cancellationToken))
            .WithMessage(ErrorMessage.ProductCategoryNotFound);
    }

    public async Task<bool> ValidateDuplicityAsync(string name, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = await _repository.GetProductsByCategoryNameAsync(name, cancellationToken);

        return !products.Any();
    }

    public async Task<bool> ValidateCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        Category? category = await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }
}
