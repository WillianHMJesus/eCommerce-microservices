using EM.Authentication.Domain.ValueObjects;
using FluentValidation;

namespace EM.Authentication.Application.Commands.SendUserToken;

public sealed class SendUserTokenCommandValidator : AbstractValidator<SendUserTokenCommand>
{
    public SendUserTokenCommandValidator()
    {
        RuleFor(x => x.EmailAddress)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Email.EmailAddressNullOrEmpty);
    }
}
