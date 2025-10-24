using EM.Catalog.Domain;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.InactivateProduct;

public sealed class InactivateProductCommandValidator : AbstractValidator<InactivateProductCommand>
{
    public InactivateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Product.InvalidProductId);
    }
}
