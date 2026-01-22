using AutoFixture;
using EM.Authentication.API.Oauth.RequestModels;

namespace EM.Authentication.BehaviorTests.Contexts.Authentication;

public class CredentialsRequestContext
{
    private readonly IFixture fixture = new Fixture();
    public const string ManagerEmailAddress = "user@manager.com";
    public const string CustomerEmailAddress = "user@customer.com";
    public const string Password = "123456Abc*";

    public CredentialsRequest GetRequest(string parameterName)
    {
        return parameterName.ToLower().Replace(" ", "") switch
        {
            "managerprofile" => GetManagerProfile(),
            "customerprofile" => GetCustomerProfile(),
            _ => throw new InvalidOperationException()
        };
    }

    private CredentialsRequest GetManagerProfile() =>
        fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, ManagerEmailAddress)
            .With(x => x.Password, Password)
            .Create();

    private CredentialsRequest GetCustomerProfile() =>
        fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, CustomerEmailAddress)
            .With(x => x.Password, Password)
            .Create();
}
