using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.Domain;
using Xunit;

namespace EM.Authentication.IntegrationTests;

[CollectionDefinition(nameof(UserCollection))]
public class UserCollection : ICollectionFixture<UserFixture> { }

#pragma warning disable CS8625
public sealed class UserFixture
{
    public const string EmailAddress = "user@manager.com";
    public const string Password = "123456Abc*";
    private readonly Faker _faker = new();

    public string AccessToken { get; set; } = "";

    public AddCustomerRequest GetValidAddCustomerRequest() =>
        new AddCustomerRequest
        {
            UserName = _faker.Name.FullName(),
            EmailAddress = _faker.Internet.Email(),
            Password = Password,
            ConfirmPassword = Password
        };

    public AddUserRequest GetValidAddUserRequest() =>
    new AddUserRequest
    {
            UserName = _faker.Name.FullName(),
            EmailAddress = _faker.Internet.Email(),
            Password = Password,
            ConfirmPassword = Password,
            ProfileName = "Manager"
        };

    public AddUserRequest GetAddUserRequestNullValues() =>
        new AddUserRequest
        {
            UserName = null,
            EmailAddress = null,
            Password = null,
            ConfirmPassword = null,
            ProfileName = null
        };

    public AddUserRequest GetAddUserRequestLargerThanMaxLenght()
    {
        var request = GetValidAddUserRequest();
        request.UserName = _faker.Random.String2(User.UserNameMaxLenght + 1);
        request.EmailAddress = _faker.Random.String2(Email.EmailAddressMaxLenght + 1);

        return request;
    }

    public AddUserRequest GetAddUserRequestInvalidValues()
    {
        var request = GetValidAddUserRequest();
        request.EmailAddress = _faker.Lorem.Word();
        request.Password = _faker.Lorem.Word();

        return request;
    }

    public AddUserRequest GetAddUserRequestDifferentPasswords()
    {
        var request = GetValidAddUserRequest();
        request.ConfirmPassword = _faker.Internet.Password();

        return request;
    }

    public AddUserRequest GetAddUserRequestProfileNameNotFound()
    {
        var request = GetValidAddUserRequest();
        request.ProfileName = _faker.Lorem.Word();

        return request;
    }

    public AuthenticateUserRequest GetValidAuthenticateUserRequest() =>
        new AuthenticateUserRequest
        {
            EmailAddress = EmailAddress,
            Password = Password
        };

    public AuthenticateUserRequest GetAuthenticateUserRequestNullValues() =>
        new AuthenticateUserRequest
        {
            EmailAddress = null,
            Password = null
        };

    public AuthenticateUserRequest GetAuthenticateUserRequestEmailAddressNotFound() =>
        new AuthenticateUserRequest
        {
            EmailAddress = _faker.Internet.Email(),
            Password = Password
        };

    public AuthenticateUserRequest GetAuthenticateUserRequestIncorrectPassword() =>
        new AuthenticateUserRequest
        {
            EmailAddress = EmailAddress,
            Password = _faker.Internet.Password(),
        };
}
#pragma warning restore CS8625