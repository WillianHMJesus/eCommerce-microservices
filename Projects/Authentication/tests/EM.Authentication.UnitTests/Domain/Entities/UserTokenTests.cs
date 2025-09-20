using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Domain.Entities;

#pragma warning disable CS8625
public sealed class UserTokenTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Validate:ValidUserToken")]
    public void Validate_ValidUserToken_ShouldNotReturnDomainException(Guid userId, string tokenHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(userId, tokenHash, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes)));

        //Assert
        domainException.Should().BeNull();
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:DefaultUserId")]
    public void Validate_EmptyUserName_ShouldReturnDomainException(string tokenHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(default, tokenHash, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes)));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.InvalidUserId);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:DefaultTokenHash")]
    public void Validate_DefaultTokenHash_ShouldReturnDomainException(Guid userId)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(userId, default, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes)));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.TokenHashNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:NullTokenHash")]
    public void Validate_NullTokenHash_ShouldReturnDomainException(Guid userId)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(userId, null, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes)));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.TokenHashNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:DefaultCreatedAt")]
    public void Validate_DefaultCreatedAt_ShouldNotReturnDomainException(Guid userId, string tokenHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(userId, tokenHash, default, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes)));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.InvalidCreationDate);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:CreationDateGreaterThanCurrentDate")]
    public void Validate_CreationDateGreaterThanCurrentDate_ShouldNotReturnDomainException(Guid userId, string tokenHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(userId, tokenHash, DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes)));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.CreationDateGreaterThanCurrentDate);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:DefaultValidatedAt")]
    public void Validate_DefaultValidatedAt_ShouldNotReturnDomainException(Guid userId, string tokenHash)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new UserToken(userId, tokenHash, DateTime.Now, default));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.InvalidExpirationDate);
    }

    [Theory, AutoUserData]
    [Trait("Test", "SetValidation")]
    public void SetValidation_ShouldSetValidationDate(Guid userId, string tokenHash)
    {
        //Arrange
        var userToken = new UserToken(userId, tokenHash, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes));

        //Act
        userToken.SetValidation();

        //Assert
        userToken.Validated.Should().BeTrue();
        userToken.ValidatedAt.Should().NotBeNull();
    }

    [Theory, AutoUserData]
    [Trait("Test", "SetUser:ValidUser")]
    public void SetUser_ValidUser_ShouldSetUser(Guid userId, string tokenHash, User user)
    {
        //Arrange
        var userToken = new UserToken(userId, tokenHash, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes));

        //Act
        userToken.SetUser(user);

        //Assert
        userToken.User.Should().NotBeNull();
        userToken.User.Should().Be(user);
    }

    [Theory, AutoUserData]
    [Trait("Test", "SetUser:NullUser")]
    public void SetUser_NullUser_ShouldReturnDomainException(Guid userId, string tokenHash)
    {
        //Arrange
        var userToken = new UserToken(userId, tokenHash, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes));

        //Act
        Exception domainException = Record.Exception(() => userToken.SetUser(null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(UserToken.InvalidUser);
    }
}
#pragma warning restore CS8625