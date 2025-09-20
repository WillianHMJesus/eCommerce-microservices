using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Application.Commands.SendUserToken;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

#pragma warning disable CS8625
public class SendUserTokenCommandSpecimenBuilder : ISpecimenBuilder
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
            "command" => new SendUserTokenCommand(_faker.Internet.Email()),
            "commanddefaultemailaddress" => new SendUserTokenCommand(default),
            "commandnullemailaddress" => new SendUserTokenCommand(null),
            _ => new NoSpecimen()
        };
    }
}
#pragma warning restore CS8625