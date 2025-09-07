using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using WH.SharedKernel;

namespace EM.Authentication.Domain;

public sealed class User : BaseEntity, IAggregateRoot
{
    public const int UserNameMaxLenght = 50;
    public const string UserNameNullOrEmpty = "The username cannot be null or empty";
    public const string PasswordHashNullOrEmpty = "The password hash cannot be null or empty";
    public const string PasswordNullOrEmpty = "The password cannot be null or empty";
    public const string InvalidPassword = "The password is invalid";
    public const string PasswordDifferent = "The password is different from the confirmation";
    public const string ProfileNull = "The user profile cannot be null";
    public const string ErrorSavingUser = "An error occurred while saving the user";
    public const string EmailAddressOrPasswordIncorrect = "The email address or password is incorrect";
    public static readonly string UserNameMaxLenghtError = $"The username cannot be greater then {UserNameMaxLenght}";

    private User() { }

    public User(string username, string emailAddress, string passwordHash)
    {
        UserName = username;
        Email = (Email)emailAddress;
        PasswordHash = passwordHash;

        Validate();
    }

    public string UserName { get; init; } = "";
    public Email Email { get; init; } = default!;
    public string PasswordHash { get; private set; } = "";
    public IList<Profile> Profiles { get; private set; } = new List<Profile>();

    public override void Validate()
    {
        Email.Validate();
        AssertionConcern.ValidateNullOrEmpty(UserName, UserNameNullOrEmpty);
        AssertionConcern.ValidateMaxLength(UserName, UserNameMaxLenght, UserNameMaxLenghtError);
        AssertionConcern.ValidateNullOrWhiteSpace(PasswordHash, PasswordHashNullOrEmpty);
    }

    public void AddProfile(Profile profile)
    {
        AssertionConcern.ValidateNullOrDefault(profile, ProfileNull);

        if (Profiles.Any(x => x.Id == profile.Id))
            return;

        Profiles.Add(profile);
    }

    public void ChangePasswordHash(string passwordHash)
    {
        AssertionConcern.ValidateNullOrWhiteSpace(passwordHash, PasswordHashNullOrEmpty);
        PasswordHash = passwordHash;
    }
}
