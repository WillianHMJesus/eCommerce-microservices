using EM.Authentication.Application;
using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Application.Mappers;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.UnitTests.AutoCustomData;
using FluentAssertions;
using Xunit;

namespace EM.Authentication.UnitTests.Application.Mappers;

public sealed class UserMapperTests
{
    [Theory, AutoUserData]
    [Trait("Test", "Map:UserToUserResponse")]
    public void Map_UserToUserResponse_ShouldReturnValidUserResponse(
        UserMapper sut,
        User user)
    {
        //Arrange & Act
        var userResponse = sut.Map(user);

        //Assert
        userResponse.Should().BeOfType<UserResponse>();
        userResponse.UserName.Should().Be(user.UserName);
        userResponse.EmailAddress.Should().Be(user.Email.EmailAddress);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Map:AddUserCommandToUser")]
    public void Map_AddUserCommandToUser_ShouldReturnValidUser(
        UserMapper sut,
        AddUserCommand command,
        string passwordHash)
    {
        //Arrange & Act
        var user = sut.Map((command, passwordHash));

        //Assert
        user.Should().BeOfType<User>();
        user.UserName.Should().Be(command.UserName);
        user.Email.EmailAddress.Should().Be(command.EmailAddress);
        user.PasswordHash.Should().Be(passwordHash);
    }

    [Theory, AutoUserData]
    [Trait("Test", "Map:UserIdAndTokenHashToUserToken")]
    public void Map_UserIdAndTokenHashToUserToken_ShouldReturnValidUser(
        UserMapper sut,
        Guid userId,
        string tokenHash)
    {
        //Arrange & Act
        var user = sut.Map((userId, tokenHash, UserToken.SecurityTokenExpirationTimeInMinutes));

        //Assert
        user.Should().BeOfType<UserToken>();
        user.UserId.Should().Be(userId);
        user.TokenHash.Should().Be(tokenHash);
    }
}
