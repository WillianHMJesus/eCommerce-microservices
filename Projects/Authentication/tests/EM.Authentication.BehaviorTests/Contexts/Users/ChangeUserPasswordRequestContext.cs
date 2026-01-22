using AutoFixture;
using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Domain.ValueObjects;

namespace EM.Authentication.BehaviorTests.Contexts.Users;

public class ChangeUserPasswordRequestContext
{
    private readonly IFixture fixture = new Fixture();
    private readonly Faker _faker = new();
    public const string ManagerEmailAddress = "user@manager.com";
    public const string CustomerEmailAddress = "user@customer.com";
    public const string Password = "123456Abc*";

    public ChangeUserPasswordRequest GetRequest(string parameterName)
    {
        return parameterName.ToLower().Replace(" ", "") switch
        {
            "managerprofile" => GetValidValues(),
            "customerprofile" => GetValidValuesCustomerProfile(),
            "validvalues" => GetValidValues(),
            "defaultvalues" => GetDefaultValues(),
            "nullvalues" => GetNullValues(),
            "emailaddressgreaterthanmaxlenght" => GetEmailAddressGreaterThanMaxLenght(),
            "invalidemailaddressandpassword" => GetInvalidEmailAddressAndPassword(),
            "passworddifferent" => GetPasswordDifferent(),
            "usernotfound" => GetUserNotFound(),
            "incorrectpassword" => GetIncorrectPassword(),
            _ => throw new InvalidOperationException()
        };
    }

    private ChangeUserPasswordRequest GetValidValues() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, ManagerEmailAddress)
            .With(x => x.OldPassword, Password)
            .With(x => x.NewPassword, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private ChangeUserPasswordRequest GetValidValuesCustomerProfile() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, CustomerEmailAddress)
            .With(x => x.OldPassword, Password)
            .With(x => x.NewPassword, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private ChangeUserPasswordRequest GetDefaultValues() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, default(string))
            .With(x => x.OldPassword, default(string))
            .With(x => x.NewPassword, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .Create();

    private ChangeUserPasswordRequest GetNullValues() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, null as string)
            .With(x => x.OldPassword, null as string)
            .With(x => x.NewPassword, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .Create();

    private ChangeUserPasswordRequest GetEmailAddressGreaterThanMaxLenght() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.OldPassword, Password)
            .With(x => x.NewPassword, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private ChangeUserPasswordRequest GetInvalidEmailAddressAndPassword() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.OldPassword, _faker.Lorem.Word())
            .With(x => x.NewPassword, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private ChangeUserPasswordRequest GetPasswordDifferent() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, ManagerEmailAddress)
            .With(x => x.OldPassword, Password)
            .With(x => x.NewPassword, Password)
            .With(x => x.ConfirmPassword, _faker.Internet.Password())
            .Create();

    private ChangeUserPasswordRequest GetUserNotFound() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.OldPassword, Password)
            .With(x => x.NewPassword, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private ChangeUserPasswordRequest GetIncorrectPassword() =>
        fixture.Build<ChangeUserPasswordRequest>()
            .With(x => x.EmailAddress, ManagerEmailAddress)
            .With(x => x.OldPassword, _faker.Internet.Password())
            .With(x => x.NewPassword, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();
}
