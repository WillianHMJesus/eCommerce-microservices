using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Application.Products.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IProductValidations _validations;
    private readonly ICategoryValidations _categoryValidations;

    public UpdateProductCommandValidator(
        IProductValidations validations,
        ICategoryValidations categoryValidations)
    {
        _validations = validations;
        _categoryValidations = categoryValidations;

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

        RuleFor(x => x.Id)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateProductIdAsync(value, cancellationToken))
            .WithMessage(Key.ProductNotFound);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateDuplicityAsync(value, cancellationToken))
            .WithMessage(Key.ProductRegisterDuplicity);

        RuleFor(x => x.CategoryId)
            .MustAsync(async (_, value, cancellationToken) => await _categoryValidations.ValidateCategoryIdAsync(value, cancellationToken))
            .WithMessage(Key.ProductCategoryNotFound);
    }
}
