using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class AuthenticateUserRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    public const string EmailAddress = "user@manager.com";
    public const string Password = "123456Abc*";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(AuthenticateUserRequest)))
        {
            return new NoSpecimen();
        }

        string AuthenticateUserRequestStringType = "EM.Authentication.API.Users.RequestModels.AuthenticateUserRequest ";
        string parameterName = request?.ToString()?.Replace(AuthenticateUserRequestStringType, "").Trim() ?? "";

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

    private AuthenticateUserRequest GetRequest() =>
        fixture.Build<AuthenticateUserRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.Password, Password)
            .Create();

    private AuthenticateUserRequest GetRequestDefaultValues() =>
        fixture.Build<AuthenticateUserRequest>()
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .Create();

    private AuthenticateUserRequest GetRequestNullValues() =>
        fixture.Build<AuthenticateUserRequest>()
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .Create();
    
    private AuthenticateUserRequest GetRequestUserNotFound() =>
        fixture.Build<AuthenticateUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .Create();

    private AuthenticateUserRequest GetIncorrectPassword() =>
    fixture.Build<AuthenticateUserRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.Password, _faker.Internet.Password())
            .Create();
}
