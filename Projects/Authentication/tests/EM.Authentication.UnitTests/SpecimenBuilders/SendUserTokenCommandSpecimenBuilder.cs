using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Application.Commands.SendUserToken;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

public class SendUserTokenCommandSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(SendUserTokenCommand)))
        {
            return new NoSpecimen();
        }

        string SendUserTokenCommandStringType = "EM.Authentication.Application.Commands.SendUserToken.SendUserTokenCommand ";
        string parameterName = request?.ToString()?.Replace(SendUserTokenCommandStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "command" => GetCommand(),
            "commanddefaultemailaddress" => GetCommandDefaultEmailAddress(),
            "commandnullemailaddress" => GetCommandNullEmailAddress(),
            _ => new NoSpecimen()
        };
    }

    private SendUserTokenCommand GetCommand() =>
        fixture.Build<SendUserTokenCommand>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .Create();

    private SendUserTokenCommand GetCommandDefaultEmailAddress() =>
        fixture.Build<SendUserTokenCommand>()
            .With(x => x.EmailAddress, default(string))
            .Create();

    private SendUserTokenCommand GetCommandNullEmailAddress() =>
        fixture.Build<SendUserTokenCommand>()
            .With(x => x.EmailAddress, null as string)
            .Create();
}