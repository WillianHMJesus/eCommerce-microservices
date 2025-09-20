using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.IntegrationTests.SpecimenBuilders;

namespace EM.Authentication.IntegrationTests.AutoCustomData;

public sealed class AutoUserDataAttribute : AutoDataAttribute
{
    public const string Password = "123456Abc*";

    public AutoUserDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        var faker = new Faker();
        var addCustomerRequest = fixture.Build<AddCustomerRequest>()
            .With(x => x.EmailAddress, faker.Internet.Email())
            .With(x => x.Password, Password)
            .With(x => x.ConfirmPassword, Password)
            .Create();

        fixture.Register(() => addCustomerRequest);
        fixture.Customizations.Add(new AddUserRequestSpecimenBuilder(fixture));
        fixture.Customizations.Add(new OauthRequestSpecimenBuilder(fixture));
        fixture.Customizations.Add(new ChangeUserPasswordRequestSpecimenBuilder(fixture));
        fixture.Customizations.Add(new SendUserTokenRequestSpecimenBuilder());
        fixture.Customizations.Add(new ValidateUserTokenRequestSpecimenBuilder());
        fixture.Customizations.Add(new ResetPasswordRequestSpecimenBuilder());

        return fixture;
    }
}
