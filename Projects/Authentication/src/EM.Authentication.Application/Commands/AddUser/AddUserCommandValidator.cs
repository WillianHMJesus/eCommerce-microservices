using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using FluentValidation;
using WH.SharedKernel;

namespace EM.Authentication.Application.Commands.AddUser;

public sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    private readonly IUserRepository _repository;

    public AddUserCommandValidator(IUserRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.UserName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(User.UserNameNullOrEmpty)
            .MaximumLength(User.UserNameMaxLenght)
            .WithMessage(User.UserNameMaxLenghtError);

        RuleFor(x => x.EmailAddress)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Email.EmailAddressNullOrEmpty)
            .MaximumLength(Email.EmailAddressMaxLenght)
            .WithMessage(Email.EmailAddressMaxLenghtError)
            .Must(ValidateEmailAddress)
            .WithMessage(Email.InvalidEmailAddress);

        RuleFor(x => x.Password)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(User.PasswordNullOrEmpty)
            .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage(User.InvalidPassword);

        RuleFor(x => x)
            .Must(x => x.Password == x.ConfirmPassword)
            .WithMessage(User.PasswordDifferent);

        RuleFor(x => x.ProfileName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(Profile.ProfileNameNullOrEmpty);

        RuleFor(x => x.EmailAddress)
            .MustAsync(ValidateDuplicityAsync)
            .WithMessage(Email.EmailAddressHasAlreadyBeenRegistered);
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

    private async Task<bool> ValidateDuplicityAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmailAsync(email, cancellationToken);

        return user is null;
    }
}
