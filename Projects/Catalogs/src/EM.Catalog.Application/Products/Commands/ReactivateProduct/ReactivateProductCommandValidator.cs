using EM.Catalog.Domain;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.ReactivateProduct;

public sealed class ReactivateProductCommandValidator : AbstractValidator<ReactivateProductCommand>
{
    public ReactivateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Product.InvalidProductId);
    }
}
