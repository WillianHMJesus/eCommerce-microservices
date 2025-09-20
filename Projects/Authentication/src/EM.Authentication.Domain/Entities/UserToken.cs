using WH.SharedKernel;

namespace EM.Authentication.Domain.Entities;

public sealed class UserToken : BaseEntity
{
    public const int SecurityTokenExpirationTimeInMinutes = 10;
    public const string InvalidUserId = "The user id is invalid";
    public const string TokenHashNullOrEmpty = "The token hash cannot be null or empty";
    public const string InvalidCreationDate = "The creation date is invalid";
    public const string InvalidExpirationDate = "The expiration date is invalid";
    public const string CreationDateGreaterThanCurrentDate = "The creation date cannot be greater than current date";
    public const string UserTokenNotFound = "The user token not found";
    public const string UserTokenNotValidated = "The user token was not validated";
    public const string InvalidUser = "The user is invalid";
    public const string ErrorSavingUserToken = "An error occurred while saving the user token";
    public const string UserTokenExpired = "The user token is expired";

    public UserToken(Guid userId, string tokenHash, DateTime createdAt, DateTime expiresAt)
    {
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;

        Validate();
    }

    public Guid UserId { get; init; }
    public string TokenHash { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
    public DateTime? ValidatedAt { get; private set; }
    public bool Validated => ValidatedAt is not null;
    public User User { get; private set; } = default!;

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrDefault(UserId, InvalidUserId);
        AssertionConcern.ValidateNullOrEmpty(TokenHash, TokenHashNullOrEmpty);
        AssertionConcern.ValidateNullOrDefault(CreatedAt, InvalidCreationDate);
        AssertionConcern.ValidateNullOrDefault(ExpiresAt, InvalidExpirationDate);

        ValidateCreationDate();
    }

    public void SetValidation()
    {
        ValidatedAt = DateTime.Now;
    }

    public void SetUser(User user)
    {
        AssertionConcern.ValidateNullOrDefault(user, InvalidUser);
        User = user;
    }

    private void ValidateCreationDate()
    {
        if (CreatedAt > DateTime.Now)
        {
            throw new DomainException(CreationDateGreaterThanCurrentDate);
        }
    }
}
