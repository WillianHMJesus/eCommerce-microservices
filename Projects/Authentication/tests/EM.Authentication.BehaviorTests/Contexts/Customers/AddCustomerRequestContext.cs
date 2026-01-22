using AutoFixture;
using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;

namespace EM.Authentication.BehaviorTests.Contexts.Customers;

public class AddCustomerRequestContext
{
    private readonly IFixture fixture = new Fixture();
    private readonly Faker _faker = new();
    public const string Password = "123456Abc*";

    public AddCustomerRequest GetRequest(string parameterName)
    {
        return parameterName.ToLower().Replace(" ", "") switch
        {
            "validvalues" => GetValidValues(),
            "defaultvalues" => GetDefaultValues(),
            "nullvalues" => GetNullValues(),
            "valuesgreaterthanmaxlenght" => GetValuesGreaterThanMaxLenght(),
            "invalidemailaddressandpassword" => GetInvalidEmailAddressAndPassword(),
            "passworddifferent" => GetPasswordDifferent(),
            _ => throw new InvalidOperationException()
        };
    }

    private AddCustomerRequest GetValidValues() =>
        fixture.Build<AddCustomerRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private AddCustomerRequest GetDefaultValues() =>
        fixture.Build<AddCustomerRequest>()
            .With(x => x.UserName, default(string))
            .With(x => x.EmailAddress, default(string))
            .With(x => x.Password, default(string))
            .With(x => x.ConfirmPassword, default(string))
            .Create();

    private AddCustomerRequest GetNullValues() =>
        fixture.Build<AddCustomerRequest>()
            .With(x => x.UserName, null as string)
            .With(x => x.EmailAddress, null as string)
            .With(x => x.Password, null as string)
            .With(x => x.ConfirmPassword, null as string)
            .Create();

    private AddCustomerRequest GetValuesGreaterThanMaxLenght() =>
        fixture.Build<AddCustomerRequest>()
            .With(x => x.UserName, _faker.Random.String2(User.UserNameMaxLenght + 1))
            .With(x => x.EmailAddress, _faker.Random.String2(Email.EmailAddressMaxLenght + 1))
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private AddCustomerRequest GetInvalidEmailAddressAndPassword() =>
        fixture.Build<AddCustomerRequest>()
            .With(x => x.EmailAddress, _faker.Lorem.Word())
            .With(x => x.Password, _faker.Lorem.Word())
            .With(x => x.ConfirmPassword, Password)
            .Create();

    private AddCustomerRequest GetPasswordDifferent() =>
        fixture.Build<AddCustomerRequest>()
            .With(x => x.EmailAddress, _faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, _faker.Lorem.Word())
            .Create();
}
