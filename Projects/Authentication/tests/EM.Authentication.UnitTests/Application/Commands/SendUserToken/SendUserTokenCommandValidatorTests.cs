using EM.Authentication.Application.Commands.SendUserToken;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands.SendUserToken;

public sealed class SendUserTokenCommandValidatorTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Constructor:ValidSendUserTokenCommand")]
    public async Task Constructor_ValidSendUserTokenCommand_ShouldReturnValidResult(
        SendUserTokenCommandValidator sut,
        SendUserTokenCommand command)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:DefaultEmailAddress")]
    public async Task Constructor_DefaultEmailAddress_ShouldReturnInvalidResult(
        SendUserTokenCommandValidator sut,
        SendUserTokenCommand commandDefaultEmailAddress)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandDefaultEmailAddress);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:NullEmailAddress")]
    public async Task Constructor_NullEmailAddress_ShouldReturnInvalidResult(
        SendUserTokenCommandValidator sut,
        SendUserTokenCommand commandNullEmailAddress)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandNullEmailAddress);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
    }
}
