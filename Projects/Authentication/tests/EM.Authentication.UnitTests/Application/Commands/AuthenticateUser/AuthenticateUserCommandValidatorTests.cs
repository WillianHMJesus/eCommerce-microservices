using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands.AuthenticateUser;

#pragma warning disable CS8625
public sealed class AuthenticateUserCommandValidatorTests
{
    [Theory, AutoUserData]
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
    public async Task Constructor_FieldsWithDefaultValues_ShouldReturnInvalidResult(
        AuthenticateUserCommandValidator sut)
    {
        //Arrange
        var command = new AuthenticateUserCommand(default, default);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        AuthenticateUserCommandValidator sut)
    {
        //Arrange
        var command = new AuthenticateUserCommand(null, null);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
    }
}
#pragma warning restore CS8625