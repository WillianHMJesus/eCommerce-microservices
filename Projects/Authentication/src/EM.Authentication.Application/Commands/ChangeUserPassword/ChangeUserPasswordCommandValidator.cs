using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.Domain;
using FluentValidation;
using WH.SharedKernel;

namespace EM.Authentication.Application.Commands.ChangeUserPassword;

public sealed class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(x => x.EmailAddress)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Email.EmailAddressNullOrEmpty)
            .MaximumLength(Email.EmailAddressMaxLenght)
            .WithMessage(Email.EmailAddressMaxLenghtError)
            .Must(ValidateEmailAddress)
            .WithMessage(Email.InvalidEmailAddress);

        RuleFor(x => x.OldPassword)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(User.PasswordNullOrEmpty);

        RuleFor(x => x.NewPassword)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(User.PasswordNullOrEmpty)
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage(User.InvalidPassword);

        RuleFor(x => x)
            .Must(x => x.NewPassword == x.ConfirmPassword)
            .WithMessage(User.PasswordDifferent);
    }

    private bool ValidateEmailAddress(string emailAddress)
    {
        try
        {
            AssertionConcern.ValidateEmailAddress(emailAddress, string.Empty);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
