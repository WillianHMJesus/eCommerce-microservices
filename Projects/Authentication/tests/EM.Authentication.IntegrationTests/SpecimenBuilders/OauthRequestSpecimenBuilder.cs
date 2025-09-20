using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Oauth.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class OauthRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    public const string EmailAddress = "user@manager.com";
    public const string Password = "123456Abc*";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(OauthRequest)))
        {
            return new NoSpecimen();
        }

        string OauthRequestStringType = "EM.Authentication.API.Oauth.RequestModels.OauthRequest ";
        string parameterName = request?.ToString()?.Replace(OauthRequestStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "request" => GetRequest(),
            "requestdefaultvalues" => GetRequestDefaultValues(),
            "requestnullvalues" => GetRequestNullValues(),
            "requestusernotfound" => GetRequestUserNotFound(),
            "requestincorrectpassword" => GetIncorrectPassword(),
            _ => new NoSpecimen()
        };
    }

    private OauthRequest GetRequest() =>
        fixture.Build<OauthRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.Password, Password)
            .Create();

    private OauthRequest GetRequestDefaultValues() =>
        fixture.Build<OauthRequest>()
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .Create();

    private OauthRequest GetRequestNullValues() =>
        fixture.Build<OauthRequest>()
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .Create();
    
    private OauthRequest GetRequestUserNotFound() =>
        fixture.Build<OauthRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .Create();

    private OauthRequest GetIncorrectPassword() =>
    fixture.Build<OauthRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.Password, _faker.Internet.Password())
            .Create();
}
