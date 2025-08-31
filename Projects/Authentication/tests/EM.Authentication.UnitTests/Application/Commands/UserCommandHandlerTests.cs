using AutoFixture.Xunit2;
using EM.Authentication.Application.Commands;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Commands.AuthenticateUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using WH.SharedKernel;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Commands;

public sealed class UserCommandHandlerTests
{
    [Theory, AutoUserData]
    public async Task Handle_AddNewUser_ShouldAddUserAndReturnSuccess(
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
    public async Task Handle_ProfileNotFound_ShouldNotAddUserAndReturnFailure(
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
    public async Task Handle_ReturnFalseCommit_ShouldNotAddUserAndReturnFailure(
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
        result.Errors.Should().Contain(x => x.Message == User.ErrorAddingUser);
    }

    [Theory, AutoUserData]
    public async Task Handle_AuthenticateUser_ShouldReturnSuccess(
        [Frozen] Mock<IPasswordHasher<UserCommandHandler>> passwordHasherMock,
        [Frozen] Mock<IConfigurationSection> configurationSectionMock,
        [Frozen] Mock<IConfiguration> configurationMock,
        UserCommandHandler sut,
        AuthenticateUserCommand command)
    {
        //Arrange
        passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<UserCommandHandler>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Success);

        configurationSectionMock.Setup(s => s.Value).Returns("360");
        configurationMock.Setup(x => x.GetSection("Jwt:ExpirationInMinutes")).Returns(configurationSectionMock.Object);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeTrue();
    }

    [Theory, AutoUserData]
    public async Task Handle_UserNotFound_ShouldReturnFailure(
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
    public async Task Handle_IncorrectPassword_ShouldReturnFailure(
        [Frozen] Mock<IPasswordHasher<UserCommandHandler>> passwordHasherMock,
        UserCommandHandler sut,
        AuthenticateUserCommand command)
    {
        //Arrange
        passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<UserCommandHandler>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(PasswordVerificationResult.Failed);

        //Act
        var result = await sut.Handle(command, CancellationToken.None);

        //Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }
}
