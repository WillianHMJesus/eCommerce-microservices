using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using EM.Authentication.Application;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.SpecimenBuilders;
using Moq;
using WH.SharedKernel;
using WH.SimpleMapper;

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
        var user = new User(
            faker.Name.FullName(),
            faker.Internet.Email(),
            faker.Random.Hash());

        var token = "Abc123";
        var userToken = new UserToken(
            Guid.NewGuid(),
            token,
            DateTime.Now,
            DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes));

        userToken.SetValidation();
        userToken.SetUser(user);

        var userResponse = fixture.Create<UserResponse>();

        fixture.Register(() => user);
        fixture.Register(() => userToken);

        fixture.Freeze<Mock<IUnitOfWork>>().Setup(x =>
            x.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<User, UserResponse>(It.IsAny<User>())).Returns(userResponse);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<(AddUserCommand Command, string PasswordHash), User>(It.IsAny<(AddUserCommand, string)>()))
            .Returns(user);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<(Guid UserId, string TokenHash, short MinutesExpire), UserToken>(It.IsAny<(Guid, string, short)>()))
            .Returns(userToken);

        fixture.Customizations.Add(new StringSpecimenBuilder());
        fixture.Customizations.Add(new AddUserCommandSpecimenBuilder(fixture));
        fixture.Customizations.Add(new AuthenticateUserCommandSpecimenBuilder(fixture));
        fixture.Customizations.Add(new ChangeUserPasswordCommandSpecimenBuilder(fixture));
        fixture.Customizations.Add(new ResetUserPasswordCommandSpecimenBuilder(fixture));
        fixture.Customizations.Add(new SendUserTokenCommandSpecimenBuilder(fixture));

        return fixture;
    }
}
