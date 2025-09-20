using EM.Authentication.Domain;
using FluentValidation;

namespace EM.Authentication.Application.Commands.ResetUserPassword;

public sealed class ResetUserPasswordCommandValidator : AbstractValidator<ResetUserPasswordCommand>
{
    public ResetUserPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(User.PasswordNullOrEmpty)
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage(User.InvalidPassword);

        RuleFor(x => x)
            .Must(x => x.NewPassword == x.ConfirmPassword)
            .WithMessage(User.PasswordDifferent);
    }
}
