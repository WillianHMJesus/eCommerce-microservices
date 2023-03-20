using EM.Catalog.Domain;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
    public AddProductCommandValidator()
	{
		RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage(ErrorMessage.ProductNameNullOrEmpty);

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .WithMessage(ErrorMessage.ProductDescriptionNullOrEmpty);

        RuleFor(x => x.Value)
            .GreaterThan(default(decimal))
            .WithMessage(ErrorMessage.ProductValueLessThanEqualToZero);

        RuleFor(x => x.Quantity)
            .GreaterThan(default(short))
            .WithMessage(ErrorMessage.ProductQuantityLessThanEqualToZero);

        RuleFor(x => x.Image)
            .NotEmpty()
            .NotNull()
            .WithMessage(ErrorMessage.ProductImageNullOrEmpty);

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty)
            .WithMessage(ErrorMessage.ProductInvalidCategoryId);
    }
}
