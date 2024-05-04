using EM.Checkout.Domain;
using FluentValidation;

namespace EM.Checkout.Application.UseCases.Purchase.Validations;

public sealed class PurchaseRequestValidator : AbstractValidator<PurchaseRequest>
{
    public PurchaseRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.UserIdInvalid);

        RuleFor(x => x.CardHolderCpf)
            .NotNull()
            .NotEmpty()
            .WithMessage(ErrorMessage.CardHolderCpfInvalid);

        RuleFor(x => x.CardHolderName)
            .NotNull()
            .NotEmpty()
            .WithMessage(ErrorMessage.CardHolderNameInvalid);

        RuleFor(x => x.CardNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage(ErrorMessage.CardNumberInvalid);

        RuleFor(x => x.CardExpirationDate)
            .NotNull()
            .NotEmpty()
            .WithMessage(ErrorMessage.CardExpirationDateInvalid);

        RuleFor(x => x.CardSecurityCode)
            .NotNull()
            .NotEmpty()
            .WithMessage(ErrorMessage.CardSecurityCodeInvalid);
    }
}
