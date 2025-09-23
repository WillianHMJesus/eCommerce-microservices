using AutoFixture.Xunit2;
using EM.Authentication.Application.Commands;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Application.Commands.ChangeUserPassword;
using EM.Authentication.Application.Commands.RefreshUserToken;
using EM.Authentication.Application.Commands.ResetUserPassword;
using EM.Authentication.Application.Commands.SendUserToken;
using EM.Authentication.Application.Commands.ValidateUserToken;
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

    [Theory, AutoUserData]
    [Trait("Test", "SendUserToken:UserTokenSentSuccessfully")]
    public async Task SendUserToken_UserTokenSentSuccessfully_ShouldReturnSuccess(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        SendUserTokenCommand command)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddTokenAsync(It.IsAny<UserToken>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    [Trait("Test", "SendUserToken:UserNotFound")]
    public async Task SendUserToken_UserNotFound_ShouldReturnSuccess(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        SendUserTokenCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddTokenAsync(It.IsAny<UserToken>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    [Trait("Test", "SendUserToken:ReturnFalseCommit")]
    public async Task SendUserToken_ReturnFalseCommit_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        SendUserTokenCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddTokenAsync(It.IsAny<UserToken>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.ErrorSavingUserToken);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserToken:UserTokenSuccessfullyValidated")]
    public async Task ValidateUserToken_UserTokenSuccessfullyValidated_ShouldReturnSuccess(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ValidateUserTokenCommand command)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateToken(It.IsAny<UserToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserToken:UserTokenNotFound")]
    public async Task ValidateUserToken_UserTokenNotFound_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ValidateUserTokenCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetTokenByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserToken);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateToken(It.IsAny<UserToken>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.UserTokenNotFound);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserToken:UserTokenExpired")]
    public async Task ValidateUserToken_UserTokenExpired_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ValidateUserTokenCommand command)
    {
        //Arrange
        var userToken = new UserToken(Guid.NewGuid(), "Abc157", DateTime.Now, DateTime.Now.AddMinutes(-1));

        repositoryMock.Setup(x => x.GetTokenByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userToken);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.UserTokenExpired);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserToken:InvalidToken")]
    public async Task ValidateUserToken_InvalidToken_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IPasswordProvider> passwordProviderMock,
        UserCommandHandler sut,
        ValidateUserTokenCommand command)
    {
        //Arrange
        passwordProviderMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.InvalidToken);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserToken:ReturnFalseCommit")]
    public async Task ValidateUserToken_ReturnFalseCommit_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ValidateUserTokenCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateToken(It.IsAny<UserToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.ErrorSavingUserToken);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPassword:UserPasswordReset")]
    public async Task ResetUserPassword_UserPasswordReset_ShouldReturnSuccess(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ResetUserPasswordCommand command)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPassword:UserTokenNotFound")]
    public async Task ResetUserPassword_UserTokenNotFound_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ResetUserPasswordCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetTokenByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserToken);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.UserTokenNotFound);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPassword:UserTokenNotValidated")]
    public async Task ResetUserPassword_UserTokenNotValidated_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ResetUserPasswordCommand command,
        string tokenHash)
    {
        //Arrange
        UserToken userToken = new(Guid.NewGuid(), tokenHash, DateTime.Now, DateTime.Now.AddMinutes(UserToken.SecurityTokenExpirationTimeInMinutes));

        repositoryMock.Setup(x => x.GetTokenByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userToken);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Never);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == UserToken.UserTokenNotValidated);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPassword:ReturnFalseCommit")]
    public async Task ResetUserPassword_ReturnFalseCommit_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        UserCommandHandler sut,
        ResetUserPasswordCommand command)
    {
        //Arrange
        unitOfWorkMock.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Update(It.IsAny<User>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.ErrorSavingUser);
    }

    [Theory, AutoUserData]
    [Trait("Test", "RefreshUserToken:ValidRefreshUserToken")]
    public async Task RefreshUserToken_ValidRefreshUserToken_ShouldReturnSuccess(
        UserCommandHandler sut,
        RefreshUserTokenCommand command)
    {
        //Arrange & Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    [Trait("Test", "RefreshUserToken:UserNotFount")]
    public async Task RefreshUserToken_UserNotFount_ShouldReturnFailure(
        [Frozen] Mock<IUserRepository> repositoryMock,
        UserCommandHandler sut,
        RefreshUserTokenCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.UserNotFound);
    }
}
