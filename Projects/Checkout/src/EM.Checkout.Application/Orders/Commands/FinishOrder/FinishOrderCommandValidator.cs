using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Checkout.Application.Orders.Commands.FinishOrder;

public sealed class FinishOrderCommandValidator : AbstractValidator<FinishOrderCommand>
{
    public FinishOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(Key.UserIdInvalid);

        RuleFor(x => x.CardHolderCpf)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CardHolderCpfInvalid);

        RuleFor(x => x.CardHolderName)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CardHolderNameInvalid);

        RuleFor(x => x.CardNumber)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CardNumberInvalid);

        RuleFor(x => x.CardExpirationDate)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CardExpirationDateInvalid);

        RuleFor(x => x.CardSecurityCode)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage(Key.CardSecurityCodeInvalid);
    }
}
