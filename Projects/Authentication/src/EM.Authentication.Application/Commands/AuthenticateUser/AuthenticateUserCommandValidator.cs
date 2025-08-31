using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using FluentValidation;

namespace EM.Authentication.Application.Commands.AuthenticateUser;

public sealed class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
{
    public AuthenticateUserCommandValidator()
    {
        RuleFor(x => x.EmailAddress)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Email.EmailAddressNullOrEmpty);

        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(User.PasswordNullOrEmpty);
    }
}
