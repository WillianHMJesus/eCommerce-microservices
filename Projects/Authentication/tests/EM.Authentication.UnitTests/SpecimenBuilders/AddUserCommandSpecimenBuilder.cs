using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.Fixtures;

namespace EM.Authentication.UnitTests.SpecimenBuilders;

public class AddUserCommandSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    private readonly string _password = PasswordFixture.GeneratePassword(12);

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(AddUserCommand)))
        {
            return new NoSpecimen();
        }

        string AddUserCommandStringType = "EM.Authentication.Application.Commands.AddUser.AddUserCommand ";
        string parameterName = request?.ToString()?.Replace(AddUserCommandStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "command" => GetCommand(),
            "commanddefaultvalues" => GetCommandDefaultValues(),
            "commandnullvalues" => GetCommandNullValues(),
            "commandgreaterthanmaxlenght" => GetCommandGreaterThanMaxLenght(),
            "invalidcommand" => GetInvalidCommand(),
            "commandpassworddifferent" =>  GetCommandPasswordDifferent(),
            _ => new NoSpecimen()
        };
    }

    private AddUserCommand GetCommand() =>
        fixture.Build<AddUserCommand>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, _password)
            .With(x => x.ConfirmPassword, _password)
            .Create();

    private AddUserCommand GetCommandDefaultValues() =>
        fixture.Build<AddUserCommand>()
            .With(x => x.UserName, default(string))
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .With(x => x.ProfileName, default(string))
            .Create();

    private AddUserCommand GetCommandNullValues() =>
        fixture.Build<AddUserCommand>()
            .With(x => x.UserName, null as string)
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .With(x => x.ProfileName, null as string)
            .Create();

    private AddUserCommand GetCommandGreaterThanMaxLenght() =>
        fixture.Build<AddUserCommand>()
            .With(x => x.UserName, _faker.Random.String2(User.UserNameMaxLenght + 1))
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.Password, _password)
            .With(x => x.ConfirmPassword, _password)
            .Create();

    private AddUserCommand GetInvalidCommand() =>
        fixture.Build<AddUserCommand>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.Password, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, _faker.Lorem.Word())
            .Create();

    private AddUserCommand GetCommandPasswordDifferent() =>
        fixture.Build<AddUserCommand>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, _password)
            .With(x => x.ConfirmPassword, PasswordFixture.GeneratePassword(12))
            .Create();
}