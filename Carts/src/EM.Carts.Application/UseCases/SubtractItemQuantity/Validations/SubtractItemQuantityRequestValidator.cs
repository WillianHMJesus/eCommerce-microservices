using EM.Carts.Domain;
using FluentValidation;

namespace EM.Carts.Application.UseCases.SubtractItemQuantity.Validations;

public class SubtractItemQuantityRequestValidator : AbstractValidator<SubtractItemQuantityRequest>
{
    public SubtractItemQuantityRequestValidator()
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
