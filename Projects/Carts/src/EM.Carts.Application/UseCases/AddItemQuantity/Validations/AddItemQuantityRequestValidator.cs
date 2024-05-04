using EM.Carts.Domain;
using FluentValidation;

namespace EM.Carts.Application.UseCases.AddItemQuantity.Validations;

public sealed class AddItemQuantityRequestValidator : AbstractValidator<AddItemQuantityRequest>
{
    public AddItemQuantityRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.UserIdInvalid);

        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.ProductIdInvalid);

        RuleFor(x => x.Quantity)
            .NotEqual(default(int))
            .WithMessage(ErrorMessage.QuantityLessThanEqualToZero);
    }
}
