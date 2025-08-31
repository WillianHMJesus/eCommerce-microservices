using AutoFixture.Xunit2;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Domain.Entities;

#pragma warning disable CS8625
public sealed class RoleTests
{
    [Theory, AutoData]
    public void Validate_ValidRole_ShouldNotReturnDomainException(string roleName)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Role(roleName));

        //Assert
        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_EmptyRoleName_ShouldReturnDomainException()
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Role(""));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Role.NameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullRoleName_ShouldReturnDomainException()
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Role(null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Role.NameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public void Validate_RoleNameLongerThanMaxLenght_ShouldReturnDomainException(string roleNameGreaterThanMaxLenght)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Role(roleNameGreaterThanMaxLenght));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Role.NameMaxLenghtError);
    }
}
#pragma warning restore CS8625
