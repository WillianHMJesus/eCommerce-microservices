using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class SendUserTokenRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    public const string EmailAddress = "user@manager.com";
    private readonly Faker _faker = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(SendUserTokenRequest)))
        {
            return new NoSpecimen();
        }

        string SendUserTokenRequestStringType = "EM.Authentication.API.Users.RequestModels.SendUserTokenRequest ";
        string parameterName = request?.ToString()?.Replace(SendUserTokenRequestStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "request" => GetRequest(),
            "requestdefaultemailaddress" => GetRequestDefaultEmailAddress(),
            "requestnullemailaddress" => GetRequestNullEmailAddress(),
            "requestusernotfound" => GetRequestUserNotFound(),
            _ => new NoSpecimen()
        };
    }

    private SendUserTokenRequest GetRequest() =>
        fixture.Build<SendUserTokenRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .Create();

    private SendUserTokenRequest GetRequestDefaultEmailAddress() =>
        fixture.Build<SendUserTokenRequest>()
            .With(x => x.EmailAddress, default(string))
            .Create();

    private SendUserTokenRequest GetRequestNullEmailAddress() =>
        fixture.Build<SendUserTokenRequest>()
            .With(x => x.EmailAddress, null as string)
            .Create();

    private SendUserTokenRequest GetRequestUserNotFound() =>
        fixture.Build<SendUserTokenRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .Create();
}