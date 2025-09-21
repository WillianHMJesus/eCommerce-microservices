using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Oauth.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class CredentialsRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    public const string EmailAddress = "user@customer.com";
    public const string Password = "123456Abc*";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(CredentialsRequest)))
        {
            return new NoSpecimen();
        }

        string CredentialsRequestStringType = "EM.Authentication.API.Oauth.RequestModels.CredentialsRequest ";
        string parameterName = request?.ToString()?.Replace(CredentialsRequestStringType, "").Trim() ?? "";

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

    private CredentialsRequest GetRequest() =>
        fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.Password, Password)
            .Create();

    private CredentialsRequest GetRequestDefaultValues() =>
        fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .Create();

    private CredentialsRequest GetRequestNullValues() =>
        fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .Create();
    
    private CredentialsRequest GetRequestUserNotFound() =>
        fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .Create();

    private CredentialsRequest GetIncorrectPassword() =>
    fixture.Build<CredentialsRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.Password, _faker.Internet.Password())
            .Create();
}
