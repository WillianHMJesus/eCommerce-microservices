using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IProductRepository _repository;

    public UpdateProductCommandValidator(IProductRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Product.InvalidProductId);

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

        RuleFor(x => x.Id)
            .MustAsync(async (_, value, cancellationToken) => await ValidateProductRegistrationAsync(value, cancellationToken))
            .WithMessage(Product.ProductNotFound);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await ValidateHasAlreadyBeenRegisteredAsync(value, cancellationToken))
            .WithMessage(Product.ProductHasAlreadyBeenRegistered);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (_, value, cancellationToken) => await ValidateCategoryRegistrationAsync(value, cancellationToken))
            .WithMessage(Category.CategoryNotFound);
    }

    public async Task<bool> ValidateProductRegistrationAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);

        return product is not null;
    }

    public async Task<bool> ValidateHasAlreadyBeenRegisteredAsync(UpdateProductCommand updateProductCommand, CancellationToken cancellationToken)
    {
        var products = await _repository.GetByNameAsync(updateProductCommand.Name, cancellationToken);

        return !products.Any(x => x.Id != updateProductCommand.Id);
    }

    private async Task<bool> ValidateCategoryRegistrationAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _repository.GetCategoryByIdAsync(categoryId, cancellationToken);

        return category is not null;
    }
}
