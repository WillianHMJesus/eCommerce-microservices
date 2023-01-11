using EM.Catalog.Domain.Entities;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.AddProduct;

public sealed class AddProductCommandValidator : AbstractValidator<AddProductCommand>
{
	public AddProductCommandValidator()
	{
		RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(Product.ErrorMessageNameNullOrEmpty);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(Product.ErrorMessageDescriptionNullOrEmpty);

        RuleFor(x => x.Value)
            .GreaterThan(default(decimal))
            .WithMessage(Product.ErrorMessageValueLessThanEqualToZero);

        RuleFor(x => x.Quantity)
            .GreaterThan(default(short))
            .WithMessage(Product.ErrorMessageQuantityLessThanEqualToZero);

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage(Product.ErrorMessageImageNullOrEmpty);
    }
}
