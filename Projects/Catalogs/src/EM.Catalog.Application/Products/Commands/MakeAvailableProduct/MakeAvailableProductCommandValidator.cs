using EM.Catalog.Application.Products.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Catalog.Application.Products.Commands.MakeAvailableProduct;

public sealed class MakeAvailableProductCommandValidator : AbstractValidator<MakeAvailableProductCommand>
{
    private readonly IProductValidations _validations;

    public MakeAvailableProductCommandValidator(IProductValidations validations)
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
