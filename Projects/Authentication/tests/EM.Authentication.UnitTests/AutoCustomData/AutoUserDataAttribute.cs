using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Domain;
using EM.Authentication.UnitTests.Fixtures;

namespace EM.Authentication.UnitTests.AutoCustomData;

public class AutoUserDataAttribute : AutoDataAttribute
{
    public AutoUserDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        Faker faker = new();
        User user = new(faker.Name.FullName(), faker.Internet.Email(), faker.Random.Hash());

        string password = PasswordFixture.GeneratePassword(12);
        var addUserCommand = fixture.Build<AddUserCommand>()
            .With(x => x.EmailAddress, faker.Internet.Email())
            .With(x => x.Password, password)
            .With(x => x.ConfirmPassword, password)
            .Create();

        var authenticateUserCommand = fixture.Build<AuthenticateUserCommand>()
            .With(x => x.EmailAddress, faker.Internet.Email())
            .With(x => x.Password, password)
            .Create();

        fixture.Register(() => user);
        fixture.Register(() => addUserCommand);
        fixture.Register(() => authenticateUserCommand);
        fixture.Customizations.Add(new CustomSpecimenBuilder());

        return fixture;
    }
}
