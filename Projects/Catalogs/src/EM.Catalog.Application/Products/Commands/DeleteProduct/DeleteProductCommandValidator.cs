using EM.Catalog.Domain;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Product.InvalidProductId);
    }
}
