using AutoFixture.Xunit2;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Moq;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands.AddUser;

#pragma warning disable CS8625
public sealed class AddUserCommandValidatorTests
{
    [Theory, AutoUserData]
    public async Task Constructor_ValidAddUserCommand_ShouldReturnValidResult(
        [Frozen] Mock<IUserRepository> repositoryMock,
        AddUserCommandValidator sut,
        AddUserCommand command)
    {
        //Arrange 
        repositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoUserData]
    public async Task Constructor_FieldsWithDefaultValues_ShouldReturnInvalidResult(
        AddUserCommandValidator sut)
    {
        //Arrange
        var command = new AddUserCommand(default, default, default, default, default);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.UserNameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Profile.ProfileNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        AddUserCommandValidator sut)
    {
        //Arrange
        var command = new AddUserCommand(null, null, null, null, null);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.UserNameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Profile.ProfileNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task Constructor_UserNameAndEmailAddressGreaterThanMaxLenght_ShouldReturnInvalidResult(
        AddUserCommandValidator sut,
        string userNameGreaterThanMaxLenght,
        string emailAddressGreaterThanMaxLenght,
        string password,
        string confirmPassword,
        string profileName)
    {
        //Arrange
        var command = new AddUserCommand(userNameGreaterThanMaxLenght, emailAddressGreaterThanMaxLenght, password, confirmPassword, profileName);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.UserNameMaxLenghtError);
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressMaxLenghtError);
    }

    [Theory, AutoUserData]
    public async Task Constructor_InvalidEmailAddressAndPassword_ShouldReturnInvalidResult(
        AddUserCommandValidator sut,
        string username,
        string invalidEmailAddress,
        string invalidPassword,
        string confirmPassword,
        string profileName)
    {
        //Arrange
        var command = new AddUserCommand(username, invalidEmailAddress, invalidPassword, confirmPassword, profileName);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.InvalidEmailAddress);
        result.Errors.Should().Contain(x => x.ErrorMessage == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    public async Task Constructor_PasswordDifferentConfirmPassword_ShouldReturnInvalidResult(
        AddUserCommandValidator sut,
        string username,
        string emailAddress,
        string password,
        string differentPassword,
        string profileName)
    {
        //Arrange
        var command = new AddUserCommand(username, emailAddress, password, differentPassword, profileName);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == User.PasswordDifferent);
    }

    [Theory, AutoUserData]
    public async Task Constructor_EmailAddressAlreadyRegistered_ShouldReturnInvalidResult(
        AddUserCommandValidator sut,
        string username,
        string emailAddress,
        string password,
        string confirmPassword,
        string profileName)
    {
        //Arrange
        var command = new AddUserCommand(username, emailAddress, password, confirmPassword, profileName);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Email.EmailAddressHasAlreadyBeenRegistered);
    }
}
#pragma warning restore CS8625