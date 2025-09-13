using AutoFixture.Xunit2;
using EM.Authentication.Application.Commands;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Application.Providers;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Moq;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands;

public sealed class UserCommandHandlerTests
{
    [Theory, AutoUserData]
    [Trait("Test", "AddUser:AddNewUser")]
    public async Task AddUser_AddNewUser_ShouldAddUserAndReturnSuccess(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        AddUserCommand command,
        User user)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(user.Id);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUser:ProfileNotFound")]
    public async Task AddUser_ProfileNotFound_ShouldNotAddUserAndReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        AddUserCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetProfileByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Profile);

        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == Profile.ProfileNotFound);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUser:ReturnFalseCommit")]
    public async Task AddUser_ReturnFalseCommit_ShouldNotAddUserAndReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        AddUserCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.ErrorSavingUser);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUser:ValidUserAuthentication")]
    public async Task AuthenticateUser_ValidUserAuthentication_ShouldReturnSuccess(
        UserCommandHandler sut,
        AuthenticateUserCommand command)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUser:UserNotFound")]
    public async Task AuthenticateUser_UserNotFound_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        UserCommandHandler sut,
        AuthenticateUserCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUser:IncorrectPassword")]
    public async Task AuthenticateUser_IncorrectPassword_ShouldReturnFailure(
        [Frozen] Mock<IPasswordProvider> passwordProviderMock,
        UserCommandHandler sut,
        AuthenticateUserCommand command)
    {
        //Arrange
        passwordProviderMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPassword:ValidUserChangePassword")]
    public async Task ChangeUserPassword_ValidUserChangePassword_ShouldChangeUserPasswordAndReturnSuccess(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ChangeUserPasswordCommand command,
        User user)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(user.Id);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPassword:UserNotFound")]
    public async Task ChangeUserPassword_UserNotFound_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        UserCommandHandler sut,
        ChangeUserPasswordCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPassword:IncorrectPassword")]
    public async Task ChangeUserPassword_IncorrectPassword_ShouldReturnFailure(
        [Frozen] Mock<IPasswordProvider> passwordProviderMock,
        UserCommandHandler sut,
        ChangeUserPasswordCommand command)
    {
        //Arrange
        passwordProviderMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPassword:ReturnFalseCommit")]
    public async Task ChangeUserPassword_ReturnFalseCommit_ShouldNotAddUserAndReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ChangeUserPasswordCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.ErrorSavingUser);
    }
}
