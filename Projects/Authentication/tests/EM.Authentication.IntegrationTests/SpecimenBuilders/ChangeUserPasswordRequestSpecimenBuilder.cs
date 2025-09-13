using AutoFixture;
using AutoFixture.Kernel;
using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Domain.ValueObjects;

namespace EM.Authentication.IntegrationTests.SpecimenBuilders;

public sealed class ChangeUserPasswordRequestSpecimenBuilder(IFixture fixture) : ISpecimenBuilder
{
    private readonly Faker _faker = new();
    public const string EmailAddress = "user@manager.com";
    public const string OldPassword = "123456Abc*";
    public const string NewPassword = "Abc123456@";

    public object Create(object request, ISpecimenContext context)
    {
        if (!request.ToString()!.Contains(nameof(ChangeUserPasswordRequest)))
        {
            return new NoSpecimen();
        }

        string ChangeUserPasswordRequestStringType = "EM.Authentication.API.Users.RequestModels.ChangeUserPasswordRequest ";
        string parameterName = request?.ToString()?.Replace(ChangeUserPasswordRequestStringType, "").Trim() ?? "";

        return parameterName.ToLower() switch
        {
            "request" => GetRequest(),
            "requestdefaultvalues" => GetRequestDefaultValues(),
            "requestnullvalues" => GetRequestNullValues(),
            "requestgreaterthanmaxlenght" => GetRequestGreaterThanMaxLenght(),
            "invalidrequest" => GetInvalidRequest(),
            "requestpassworddifferent" => GetRequestPasswordDifferent(),
            "requestusernotfound" => GetRequestUserNotFound(),
            "requestincorrectpassword" => GetRequestIncorrectPassword(),
            _ => new NoSpecimen()
        };
    }

    private ChangeUserPasswordRequest GetRequest() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.OldPassword, OldPassword)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ChangeUserPasswordRequest GetRequestDefaultValues() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, default(string))
            .With(x => x.OldPassword, default(string))
            .With(x => x.NewPassword, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .Create();

    private ChangeUserPasswordRequest GetRequestNullValues() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, null as string)
            .With(x => x.OldPassword, null as string)
            .With(x => x.NewPassword, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .Create();

    private ChangeUserPasswordRequest GetRequestGreaterThanMaxLenght() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.OldPassword, OldPassword)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ChangeUserPasswordRequest GetInvalidRequest() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.OldPassword, _faker.Lorem.Word())
            .With(x => x.NewPassword, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ChangeUserPasswordRequest GetRequestPasswordDifferent() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.OldPassword, OldPassword)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, _faker.Internet.Password())
            .Create();

    private ChangeUserPasswordRequest GetRequestUserNotFound() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.OldPassword, OldPassword)
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();

    private ChangeUserPasswordRequest GetRequestIncorrectPassword() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, EmailAddress)
            .With(x => x.OldPassword, _faker.Internet.Password())
            .With(x => x.NewPassword, NewPassword)
            .With(x => x.ConfirmPassword, NewPassword)
            .Create();
}
