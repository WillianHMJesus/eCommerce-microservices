using EM.Authentication.Application.Commands.ResetUserPassword;
using EM.Authentication.Domain;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands.ResetUserPassword;

public sealed class ResetUserPasswordCommandValidatorTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Constructor:ValidResetUserPasswordCommand")]
    public async Task Constructor_ValidResetUserPasswordCommand_ShouldReturnValidResult(
        ResetUserPasswordCommandValidator sut,
        ResetUserPasswordCommand command)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:NewDefaultPassword")]
    public async Task Constructor_NewDefaultPassword_ShouldReturnInvalidResult(
        ResetUserPasswordCommandValidator sut,
        ResetUserPasswordCommand commandNewDefaultPassword)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandNewDefaultPassword);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:NewNullPassword")]
    public async Task Constructor_NewNullPassword_ShouldReturnInvalidResult(
        ResetUserPasswordCommandValidator sut,
        ResetUserPasswordCommand commandNewNullPassword)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandNewNullPassword);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:NewInvalidaPassword")]
    public async Task Constructor_NewInvalidaPassword_ShouldReturnInvalidResult(
        ResetUserPasswordCommandValidator sut,
        ResetUserPasswordCommand commandNewInvalidaPassword)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandNewInvalidaPassword);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:PasswordDifferentConfirmPassword")]
    public async Task Constructor_PasswordDifferentConfirmPassword_ShouldReturnInvalidResult(
        ResetUserPasswordCommandValidator sut,
        ResetUserPasswordCommand commandPasswordDifferent)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandPasswordDifferent);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordDifferent);
    }
}
