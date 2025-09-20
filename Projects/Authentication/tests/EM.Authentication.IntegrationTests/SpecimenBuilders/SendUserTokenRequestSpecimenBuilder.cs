using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

#pragma warning disable CS8625
public sealed class SendUserTokenRequestSpecimenBuilder : ISpecimenBuilder
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
            "request" => new SendUserTokenRequest { EmailAddress = EmailAddress },
            "requestdefaultemailaddress" => new SendUserTokenRequest { EmailAddress = default },
            "requestnullemailaddress" => new SendUserTokenRequest { EmailAddress = null },
            "requestusernotfound" => new SendUserTokenRequest { EmailAddress = _faker.Internet.Email() },
            _ => new NoSpecimen()
        };
    }
}
#pragma warning restore CS8625