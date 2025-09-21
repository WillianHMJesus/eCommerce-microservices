using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Application.Commands.ResetUserPassword;
using EM.Authentication.UnitTests.Fixtures;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

public class ResetUserPasswordCommandSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
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
            "command" => GetCommand(),
            "commandnewdefaultpassword" => GetCommandDefaultNewPassword(),
            "commandnewnullpassword" => GetCommandNullNewPassword(),
            "commandnewinvalidapassword" => GetCommandInvalidPassword(),
            "commandpassworddifferent" => GetCommandPasswordDifferent(),
            _ => new NoSpecimen()
        };
    }

    private ResetUserPasswordCommand GetCommand() =>
        fixture.Build<ResetUserPasswordCommand>()
            .With(x => x.NewPassword, _password)
            .With(x => x.ConfirmPassword, _password)
            .Create();

    private ResetUserPasswordCommand GetCommandDefaultNewPassword() =>
        fixture.Build<ResetUserPasswordCommand>()
            .With(x => x.NewPassword, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .Create();

    private ResetUserPasswordCommand GetCommandNullNewPassword() =>
        fixture.Build<ResetUserPasswordCommand>()
            .With(x => x.NewPassword, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .Create();

    private ResetUserPasswordCommand GetCommandInvalidPassword() =>
        fixture.Build<ResetUserPasswordCommand>()
            .With(x => x.NewPassword, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, _password)
            .Create();

    private ResetUserPasswordCommand GetCommandPasswordDifferent() =>
        fixture.Build<ResetUserPasswordCommand>()
            .With(x => x.NewPassword, _password)
            .With(x => x.ConfirmPassword, PasswordFixture.GeneratePassword(12))
            .Create();
}