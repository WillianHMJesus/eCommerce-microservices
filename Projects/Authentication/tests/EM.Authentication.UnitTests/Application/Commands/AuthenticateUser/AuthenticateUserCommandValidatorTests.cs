using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands.AuthenticateUser;

public sealed class AuthenticateUserCommandValidatorTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Constructor:ValidAuthenticateUserCommand")]
    public async Task Constructor_ValidAuthenticateUserCommand_ShouldReturnValidResult(
        AuthenticateUserCommandValidator sut,
        AuthenticateUserCommand command)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:FieldsWithDefaultValues")]
    public async Task Constructor_FieldsWithDefaultValues_ShouldReturnInvalidResult(
        AuthenticateUserCommandValidator sut,
        AuthenticateUserCommand commandDefaultValues)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandDefaultValues);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:FieldsWithNullValues")]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        AuthenticateUserCommandValidator sut,
        AuthenticateUserCommand commandNullValues)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandNullValues);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
    }
}