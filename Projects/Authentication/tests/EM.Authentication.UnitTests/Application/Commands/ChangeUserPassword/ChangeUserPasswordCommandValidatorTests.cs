using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands.ChangeUserPassword;

#pragma warning disable CS8625
public sealed class ChangeUserPasswordCommandValidatorTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Constructor:ValidChangeUserPasswordCommand")]
    public async Task Constructor_ValidChangeUserPasswordCommand_ShouldReturnValidResult(
        ChangeUserPasswordCommandValidator sut,
        ChangeUserPasswordCommand command)
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
        ChangeUserPasswordCommandValidator sut,
        ChangeUserPasswordCommand commandDefaultValues)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandDefaultValues);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
        result.Errors.Where(x => x.ErrorMessage == User.PasswordNullOrEmpty).Count().Should().Be(2);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:FieldsWithNullValues")]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        ChangeUserPasswordCommandValidator sut,
        ChangeUserPasswordCommand commandNullValues)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandNullValues);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
        result.Errors.Where(x => x.ErrorMessage == User.PasswordNullOrEmpty).Count().Should().Be(2);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:EmailAddressGreaterThanMaxLenght")]
    public async Task Constructor_EmailAddressGreaterThanMaxLenght_ShouldReturnInvalidResult(
        ChangeUserPasswordCommandValidator sut,
        ChangeUserPasswordCommand commandGreaterThanMaxLenght)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandGreaterThanMaxLenght);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressMaxLenghtError);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:InvalidEmailAddressAndPassword")]
    public async Task Constructor_InvalidEmailAddressAndPassword_ShouldReturnInvalidResult(
        ChangeUserPasswordCommandValidator sut,
        ChangeUserPasswordCommand invalidCommand)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(invalidCommand);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.InvalidEmailAddress);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Constructor:PasswordDifferentConfirmPassword")]
    public async Task Constructor_PasswordDifferentConfirmPassword_ShouldReturnInvalidResult(
        ChangeUserPasswordCommandValidator sut,
        ChangeUserPasswordCommand commandPasswordDifferent)
    {
        //Arrange & Act
        var result = await sut.ValidateAsync(commandPasswordDifferent);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordDifferent);
    }
}
#pragma warning restore CS8625