using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IReadRepository _repository;

    public UpdateProductCommandValidator(IReadRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Key.ProductInvalidId);

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

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(Key.ProductRegisterDuplicity);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryIdAsync(value, cancellationToken))
            .WithMessage(Key.ProductCategoryNotFound);
    }

    public async Task<bool> ValidateDuplicityAsync(UpdateProductCommand updateProductCommand, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = await _repository.GetProductsByNameAsync(updateProductCommand.Name, cancellationToken);

        return !products.Any(x => x.Id != updateProductCommand.Id);
    }

    public async Task<bool> ValidateCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        Category? category = await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }
}
