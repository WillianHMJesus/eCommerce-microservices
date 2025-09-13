using AutoFixture;
using AutoFixture.Kernel;
using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.Fixtures;
using Bogus;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

#pragma warning disable CS8625
public class ChangeUserPasswordCommandSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    private readonly string _password = PasswordFixture.GeneratePassword(12);

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(ChangeUserPasswordCommand)))
        {
            return new NoSpecimen();
        }

        string ChangeUserPasswordCommandStringType = "EM.Authentication.Application.Commands.ChangeUserPassword.ChangeUserPasswordCommand ";
        string parameterName = request?.ToString()?.Replace(ChangeUserPasswordCommandStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "command" => GetCommand(),
            "commanddefaultvalues" => new ChangeUserPasswordCommand(default, default, default, default),
            "commandnullvalues" => new ChangeUserPasswordCommand(null, null, null, null),
            "commandgreaterthanmaxlenght" => GetCommandGreaterThanMaxLenght(),
            "invalidcommand" => GetInvalidCommand(),
            "commandpassworddifferent" => GetCommandPasswordDifferent(),
            _ => new NoSpecimen()
        };
    }

    private ChangeUserPasswordCommand GetCommand() =>
        fixture.Build<ChangeUserPasswordCommand>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.NewPassword, _password)
            .With(x => x.ConfirmPassword, _password)
            .Create();

    private ChangeUserPasswordCommand GetCommandGreaterThanMaxLenght() =>
        fixture.Build<ChangeUserPasswordCommand>()
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.NewPassword, _password)
            .With(x => x.ConfirmPassword, _password)
            .Create();

    private ChangeUserPasswordCommand GetInvalidCommand() =>
        fixture.Build<ChangeUserPasswordCommand>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.NewPassword, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, _faker.Lorem.Word())
            .Create();

    private ChangeUserPasswordCommand GetCommandPasswordDifferent() =>
        fixture.Build<ChangeUserPasswordCommand>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.NewPassword, _password)
            .With(x => x.ConfirmPassword, PasswordFixture.GeneratePassword(12))
            .Create();
}
#pragma warning restore CS8625