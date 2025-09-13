using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using System.Net.Mail;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Domain.ValueObjects;

#pragma warning disable CS8625
#pragma warning disable CS8602
public sealed class EmailTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Validate:ValidEmail")]
    public void Validate_ValidEmail_ShouldNotReturnDomainException(string emailAddress)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Email(emailAddress));

        //Assert
        domainException.Should().BeNull();
    }

    [Fact]
    [Trait("Test", "Validate:EmptyEmailAddress")]
    public void Validate_EmptyEmailAddress_ShouldReturnDomainException()
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Email(""));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Email.EmailAddressNullOrEmpty);
    }

    [Fact]
    [Trait("Test", "Validate:NullEmailAddress")]
    public void Validate_NullEmailAddress_ShouldReturnDomainException()
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Email(null));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Email.EmailAddressNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:EmailAddressLongerThanMaxLenght")]
    public void Validate_EmailAddressLongerThanMaxLenght_ShouldReturnDomainException(string emailAddressGreaterThanMaxLenght)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Email(emailAddressGreaterThanMaxLenght));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Email.EmailAddressMaxLenghtError);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Validate:InvalidEmailAddress")]
    public void Validate_InvalidEmailAddress_ShouldReturnDomainException(string invalidEmail)
    {
        //Arrange & Act
        Exception domainException = Record.Exception(() => new Email(invalidEmail));

        //Assert
        domainException.Should().NotBeNull();
        domainException.Should().BeOfType<DomainException>();
        domainException.Message.Should().Be(Email.InvalidEmailAddress);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Emails:WithSameEmailAddress")]
    public void Emails_WithSameEmailAddress_ShouldBeEqual(string emailAddress)
    {
        // Arrange
        var originalEmail = new Email(emailAddress);
        var comparisonEmail = new Email(emailAddress);

        // Act & Assert
        Assert.Equal(originalEmail, comparisonEmail);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Emails:WithDifferentEmailAddress")]
    public void Emails_WithDifferentEmailAddress_ShouldNotBeEqual(MailAddress originalMail, MailAddress comparisonMail)
    {
        // Arrange
        var originalEmail = new Email(originalMail.Address);
        var comparisonEmail = new Email(comparisonMail.Address);

        // Act & Assert
        Assert.NotEqual(originalEmail, comparisonEmail);
    }

    [Theory, AutoUserData]
    [Trait("Test", "GetEqualityComponents:ValidEmail")]
    public void GetEqualityComponents_ValidEmail_ShouldReturnEmailAddress(string emailAddress)
    {
        // Arrange
        var email = new Email(emailAddress);

        // Act
        var components = email
            .GetType()
            .GetMethod("GetEqualityComponents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(email, null) as IEnumerable<object>;

        // Assert
        components.Should().Contain(emailAddress);
    }
}
#pragma warning restore CS8625
#pragma warning restore CS8602