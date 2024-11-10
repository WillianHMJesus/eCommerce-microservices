using EM.Catalog.Application.Products.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    private readonly IProductValidations _validations;

    public DeleteProductCommandValidator(IProductValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage(Key.ProductInvalidId);

        RuleFor(x => x.Id)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateProductIdAsync(value, cancellationToken))
            .WithMessage(Key.ProductNotFound);
    }
}
