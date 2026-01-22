using AutoFixture;
using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;

namespace EM.Authentication.BehaviorTests.Contexts.Users;

public class AddUserRequestContext
{
    private readonly IFixture fixture = new Fixture();
    private readonly Faker _faker = new();
    public const string Password = "123456Abc*";
    public const string ManagerProfile = "Manager";

    public AddUserRequest GetRequest(string parameterName)
    {
        return parameterName.ToLower().Replace(" ", "") switch
        {
            "validvalues" => GetValidValues(),
            "defaultvalues" => GetDefaultValues(),
            "nullvalues" => GetNullValues(),
            "valuesgreaterthanmaxlenght" => GetValuesGreaterThanMaxLenght(),
            "invalidemailaddressandpassword" => GetInvalidEmailAddressAndPassword(),
            "passworddifferent" => GetPasswordDifferent(),
            "profilenotfound" => GetProfileNotFound(),
            _ => throw new InvalidOperationException()
        };
    }

    private AddUserRequest GetValidValues() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, ManagerProfile)
            .Create();

    private AddUserRequest GetDefaultValues() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.UserName, default(string))
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .With(x => x.ProfileName, default(string))
            .Create();

    private AddUserRequest GetNullValues() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.UserName, null as string)
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .With(x => x.ProfileName, null as string)
            .Create();

    private AddUserRequest GetValuesGreaterThanMaxLenght() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.UserName, _faker.Random.String2(User.UserNameMaxLenght + 1))
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, ManagerProfile)
            .Create();

    private AddUserRequest GetInvalidEmailAddressAndPassword() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.Password, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, Password)
            .With(x => x.ProfileName, ManagerProfile)
            .Create();

    private AddUserRequest GetPasswordDifferent() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, _faker.Lorem.Word())
            .With(x => x.ProfileName, ManagerProfile)
            .Create();

    private AddUserRequest GetProfileNotFound() =>
        fixture.Build<AddUserRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();
}
