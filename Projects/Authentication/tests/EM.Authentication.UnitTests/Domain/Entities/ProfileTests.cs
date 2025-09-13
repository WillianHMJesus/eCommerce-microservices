using AutoFixture.Xunit2;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Domain.Entities;

#pragma warning disable CS8625
public sealed class ProfileTests
{
    [Theory, AutoData]
    [Trait("Test", "Validate:ValidProfile")]
    public void Validate_ValidProfile_ShouldNotReturnDomainException(string profileName)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Profile(profileName));

        //Assert
        domainException.Should().BeNull();
    }

    [Fact]
    [Trait("Test", "Validate:EmptyProfileName")]
    public void Validate_EmptyProfileName_ShouldReturnDomainException()
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Profile(""));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Profile.NameNullOrEmpty);
    }

    [Fact]
    [Trait("Test", "Validate:NullProfileName")]
    public void Validate_NullProfileName_ShouldReturnDomainException()
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Profile(null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Profile.NameNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:ProfileNameLongerThanMaxLenght")]
    public void Validate_ProfileNameLongerThanMaxLenght_ShouldReturnDomainException(string profileNameGreaterThanMaxLenght)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Profile(profileNameGreaterThanMaxLenght));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Profile.NameMaxLenghtError);
    }
}
#pragma warning restore CS8625
