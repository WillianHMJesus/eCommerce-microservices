using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Application.Commands.ResetUserPassword;
using EM.Authentication.UnitTests.Fixtures;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

#pragma warning disable CS8625
public class ResetUserPasswordCommandSpecimenBuilder : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    private readonly string _password = PasswordFixture.GeneratePassword(12);

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(ResetUserPasswordCommand)))
        {
            return new NoSpecimen();
        }

        string ResetUserPasswordCommandStringType = "EM.Authentication.Application.Commands.ResetUserPassword.ResetUserPasswordCommand ";
        string parameterName = request?.ToString()?.Replace(ResetUserPasswordCommandStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "command" => new ResetUserPasswordCommand(Guid.NewGuid(), _password, _password),
            "commandnewdefaultpassword" => new ResetUserPasswordCommand(Guid.NewGuid(), default, _password),
            "commandnewnullpassword" => new ResetUserPasswordCommand(Guid.NewGuid(), null, _password),
            "commandnewinvalidapassword" => new ResetUserPasswordCommand(Guid.NewGuid(), _faker.Lorem.Word(), _password),
            "commandpassworddifferent" => new ResetUserPasswordCommand(Guid.NewGuid(), _password, PasswordFixture.GeneratePassword(12)),
            _ => new NoSpecimen()
        };
    }
}
#pragma warning restore CS8625