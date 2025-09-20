using EM.Authentication.API.Oauth.RequestModels;
using Xunit;

namespace EM.Authentication.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(UserCollection))]
public class UserCollection : ICollectionFixture<UserFixture> { }

public sealed class UserFixture : BaseFixture
{
    public const string ManagerEmailAddress = "user@manager.com";
    public const string CustomerEmailAddress = "user@customer.com";
    public const string Password = "123456Abc*";

    public string AccessToken { get; set; } = "";

    public static OauthRequest GetManagerAuthenticateRequest() =>
        new()
        {
            EmailAddress = ManagerEmailAddress,
            Password = Password
        };

    public static OauthRequest GetCustomerAuthenticateRequest() =>
        new()
        {
            EmailAddress = CustomerEmailAddress,
            Password = Password
        };
}