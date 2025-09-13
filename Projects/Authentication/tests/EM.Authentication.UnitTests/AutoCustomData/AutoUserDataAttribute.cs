using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using EM.Authentication.Domain;
using EM.Authentication.UnitTests.SpecimenBuilders;
using Moq;
using WH.SharedKernel;

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

        fixture.Customizations.Add(new StringSpecimenBuilder());
        fixture.Customizations.Add(new AddUserCommandSpecimenBuilder(fixture));
        fixture.Customizations.Add(new AuthenticateUserCommandSpecimenBuilder(fixture));
        fixture.Customizations.Add(new ChangeUserPasswordCommandSpecimenBuilder(fixture));

        User user = new(faker.Name.FullName(), faker.Internet.Email(), faker.Random.Hash());
        fixture.Register(() => user);

        fixture.Freeze<Mock<IUnitOfWork>>().Setup(x =>
            x.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        return fixture;
    }
}
