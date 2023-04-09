using EM.Carts.Domain;
using FluentValidation;

namespace EM.Carts.Application.UseCases.AddItemQuantity.Validations;

public class AddItemQuantityRequestValidator : AbstractValidator<AddItemQuantityRequest>
{
    public AddItemQuantityRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.ProductIdInvalid);

        RuleFor(x => x.Quantity)
            .NotEqual(default(int))
            .WithMessage(ErrorMessage.QuantityLessThanEqualToZero);
    }
}
