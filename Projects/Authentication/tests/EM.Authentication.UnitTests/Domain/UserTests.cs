using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using System.Reflection;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Domain;

#pragma warning disable CS8625
#pragma warning disable CS8600
public sealed class UserTests
{
    [Theory, AutoUserData]
    public void Validate_ValidUser_ShouldNotReturnDomainException(string username, string emailAddress, string passwordHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new User(username, emailAddress, passwordHash));

        //Assert
        domainException.Should().BeNull();
    }

    [Theory, AutoUserData]
    public void Validate_EmptyUserName_ShouldReturnDomainException(string emailAddress, string passwordHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new User("", emailAddress, passwordHash));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(User.UserNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public void Validate_NullUserName_ShouldReturnDomainException(string emailAddress, string passwordHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new User(null, emailAddress, passwordHash));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(User.UserNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public void Validate_UserNameGreaterThanMaxLenght_ShouldReturnDomainException(string usernameGreaterThanMaxLenght, string emailAddress, string passwordHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new User(usernameGreaterThanMaxLenght, emailAddress, passwordHash));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(User.UserNameMaxLenghtError);
    }

    [Theory, AutoUserData]
    public void Validate_EmptyPasswordHash_ShouldReturnDomainException(string username, string emailAddress)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new User(username, emailAddress, ""));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(User.PasswordHashNullOrEmpty);
    }

    [Theory, AutoUserData]
    public void Validate_NullPasswordHash_ShouldReturnDomainException(string username, string emailAddress)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new User(username, emailAddress, null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(User.PasswordHashNullOrEmpty);
    }

    [Theory, AutoUserData]
    public void AddProfile_NewValidProfile_ShouldAddTheNewProfileInUser(User user, Profile profile)
    {
        //Arrange & Act
        user.AddProfile(profile);

        //Assert
        user.Profiles.Should().HaveCount(1);
        user.Profiles.Should().Contain(x => x.Id == profile.Id);
    }

    [Theory, AutoUserData]
    public void AddProfile_ExistingValidProfile_ShouldNotAddTheNewProfileInUser(User user, Profile profile)
    {
        //Arrange
        user.AddProfile(profile);

        //Act
        user.AddProfile(profile);

        //Assert
        user.Profiles.Should().HaveCount(1);
        user.Profiles.Should().Contain(x => x.Id == profile.Id);
    }

    [Theory, AutoUserData]
    public void AddProfile_NewInvalidProfile_ShouldReturnDomainException(User user)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => user.AddProfile(null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(User.ProfileNull);
    }

    [Fact]
    public void Constructor_EmptyPrivateConstructor_ShouldReturnValidInstance()
    {
        //Arrange & Act
        var instance = (User)Activator.CreateInstance(
            typeof(User),
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            null,
            null);

        //Assert
        instance.Should().NotBe(null);
    }
}
#pragma warning restore CS8625
#pragma warning restore CS8600