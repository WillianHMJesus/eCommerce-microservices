using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.UnitTests.Fixtures;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

#pragma warning disable CS8625
public class AuthenticateUserCommandSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    private readonly string _password = PasswordFixture.GeneratePassword(12);

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(AuthenticateUserCommand)))
        {
            return new NoSpecimen();
        }

        string AuthenticateUserCommandStringType = "EM.Authentication.Application.Commands.AuthenticateUser.AuthenticateUserCommand ";
        string parameterName = request?.ToString()?.Replace(AuthenticateUserCommandStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "command" => GetCommand(),
            "commanddefaultvalues" => new AuthenticateUserCommand(default, default),
            "commandnullvalues" => new AuthenticateUserCommand(null, null),
            _ => new NoSpecimen()
        };
    }

    private AuthenticateUserCommand GetCommand() =>
        fixture.Build<AuthenticateUserCommand>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, _password)
            .Create();
}
#pragma warning restore CS8625