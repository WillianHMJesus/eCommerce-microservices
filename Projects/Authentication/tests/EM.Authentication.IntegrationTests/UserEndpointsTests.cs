using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Application;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.IntegrationTests.AutoCustomData;
using EM.Authentication.IntegrationTests.Fixtures;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace EM.Authentication.IntegrationTests;

[Collection(nameof(UserCollection))]
public sealed class UserEndpointsTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly UserFixture _fixture;

    public UserEndpointsTests(
        IntegrationTestWebAppFactory factory,
        UserFixture fixture)
    {
        _client = factory.CreateClient();
        _fixture = fixture;

        if (string.IsNullOrEmpty(fixture.AccessToken))
            fixture.AccessToken = GetManagerAccessToken().Result;

        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {fixture.AccessToken}");
    }

    #region AddCustomer
    [Theory, AutoUserData]
    [Trait("Test", "AddCustomerAsync:ValidCustomer")]
    public async Task AddCustomerAsync_ValidCustomer_ShouldReturnStatusCodeOk(AddCustomerRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/customer-profile", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion

    #region AddUser
    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:ValidUser")]
    public async Task AddUserAsync_ValidUser_ShouldReturnStatusCodeOk(AddUserRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:WithoutAccessToken")]
    public async Task AddUserAsync_WithoutAccessToken_ShouldReturnStatusCodeUnauthorized(AddUserRequest request)
    {
        //Arrange
        _client.DefaultRequestHeaders.Clear();
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:AccessTokenForbidden")]
    public async Task AddUserAsync_AccessTokenForbidden_ShouldReturnStatusCodeForbidden(AddUserRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);
        string accessToken = await GetCustomerAccessToken();

        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:DefaultValues")]
    public async Task AddUserAsync_DefaultValues_ShouldReturnStatusCodeBadRequest(AddUserRequest requestDefaultValues)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestDefaultValues);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.UserNameNullOrEmpty);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
        errors.Should().Contain(x => x.Message == Profile.ProfileNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:NullValues")]
    public async Task AddUserAsync_NullValues_ShouldReturnStatusCodeBadRequest(AddUserRequest requestNullValues)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestNullValues);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.UserNameNullOrEmpty);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
        errors.Should().Contain(x => x.Message == Profile.ProfileNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:UserNameAndEmailAddressLargerThanMaxLenght")]
    public async Task AddUserAsync_UserNameAndEmailAddressLargerThanMaxLenght_ShouldReturnStatusCodeBadRequest(AddUserRequest requestGreaterThanMaxLenght)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestGreaterThanMaxLenght);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.UserNameMaxLenghtError);
        errors.Should().Contain(x => x.Message == Email.EmailAddressMaxLenghtError);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:InvalidUserNameAndEmailAddress")]
    public async Task AddUserAsync_InvalidUserNameAndEmailAddress_ShouldReturnStatusCodeBadRequest(AddUserRequest invalidRequest)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(invalidRequest);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.InvalidEmailAddress);
        errors.Should().Contain(x => x.Message == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:DifferentPasswords")]
    public async Task AddUserAsync_DifferentPasswords_ShouldReturnStatusCodeBadRequest(AddUserRequest requestPasswordDifferent)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestPasswordDifferent);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.PasswordDifferent);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AddUserAsync:ProfileNameNotFound")]
    public async Task AddUserAsync_ProfileNameNotFound_ShouldReturnStatusCodeBadRequest(AddUserRequest requestProfileNameNotFound)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestProfileNameNotFound);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Profile.ProfileNotFound);
    }
    #endregion

    #region ChangeUserPassword
    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:ValidPassword")]
    public async Task ChangeUserPasswordAsync_ValidPassword_ShouldReturnStatusCodeOk(ChangeUserPasswordRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:WithoutAccessToken")]
    public async Task ChangeUserPasswordAsync_WithoutAccessToken_ShouldReturnStatusCodeUnauthorized(ChangeUserPasswordRequest request)
    {
        //Arrange
        _client.DefaultRequestHeaders.Clear();
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:DefaultValues")]
    public async Task ChangeUserPasswordAsync_DefaultValues_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest requestDefaultValues)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestDefaultValues);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:NullValues")]
    public async Task ChangeUserPasswordAsync_NullValues_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest requestNullValues)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestNullValues);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:EmailAddressLargerThanMaxLenght")]
    public async Task ChangeUserPasswordAsync_EmailAddressLargerThanMaxLenght_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest requestGreaterThanMaxLenght)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestGreaterThanMaxLenght);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressMaxLenghtError);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:InvalidEmailAddressAndPassword")]
    public async Task ChangeUserPasswordAsync_InvalidEmailAddressAndPassword_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest invalidRequest)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(invalidRequest);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.InvalidEmailAddress);
        errors.Should().Contain(x => x.Message == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:DifferentPasswords")]
    public async Task ChangeUserPasswordAsync_DifferentPasswords_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest requestPasswordDifferent)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestPasswordDifferent);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.PasswordDifferent);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:UserNotFound")]
    public async Task ChangeUserPasswordAsync_UserNotFound_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest requestUserNotFound)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserNotFound);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:IncorrectPassword")]
    public async Task ChangeUserPasswordAsync_IncorrectPassword_ShouldReturnStatusCodeBadRequest(ChangeUserPasswordRequest requestIncorrectPassword)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestIncorrectPassword);

        //Act
        HttpResponseMessage response = await _client.PutAsync("/api/users/change-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }
    #endregion

    #region SendUserToken
    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:ValidEmailAddress")]
    public async Task SendUserTokenAsync_ValidEmailAddress_ShouldReturnStatusCodeOk(SendUserTokenRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/send-token", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:DefaultEmailAddress")]
    public async Task SendUserTokenAsync_DefaultEmailAddress_ShouldReturnStatusCodeBadRequest(SendUserTokenRequest requestDefaultEmailAddress)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestDefaultEmailAddress);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/send-token", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:NullEmailAddress")]
    public async Task SendUserTokenAsync_NullEmailAddress_ShouldReturnStatusCodeBadRequest(SendUserTokenRequest requestNullEmailAddress)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestNullEmailAddress);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/send-token", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ChangeUserPasswordAsync:UserNotFound")]
    public async Task SendUserTokenAsync_UserNotFound_ShouldReturnStatusCodeOk(SendUserTokenRequest requestUserNotFound)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserNotFound);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/send-token", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    #endregion

    #region ValidateUserToken
    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserTokenAsync:ValidUserToken")]
    public async Task ValidateUserTokenAsync_ValidUserToken_ShouldReturnStatusCodeOk(ValidateUserTokenRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/validate-token", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserTokenAsync:UserTokenNotFound")]
    public async Task ValidateUserTokenAsync_UserTokenNotFound_ShouldReturnStatusCodeBadRequest(ValidateUserTokenRequest requestUserTokenNotFound)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserTokenNotFound);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/validate-token", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == UserToken.UserTokenNotFound);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ValidateUserTokenAsync:UserTokenExpired")]
    public async Task ValidateUserTokenAsync_UserTokenExpired_ShouldReturnStatusCodeBadRequest(ValidateUserTokenRequest requestUserTokenExpired)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserTokenExpired);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/validate-token", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == UserToken.UserTokenExpired);
    }
    #endregion

    #region ResetUserPassword
    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:ValidUserToken")]
    public async Task ResetUserPasswordAsync_ValidUserToken_ShouldReturnStatusCodeOk(ResetPasswordRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:DefaultNewPassword")]
    public async Task ResetUserPasswordAsync_DefaultNewPassword_ShouldReturnStatusCodeBadRequest(ResetPasswordRequest requestDefaultNewPassword)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestDefaultNewPassword);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:NullNewPassword")]
    public async Task ResetUserPasswordAsync_NullNewPassword_ShouldReturnStatusCodeBadRequest(ResetPasswordRequest requestNullNewPassword)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestNullNewPassword);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:InvalidNewPassword")]
    public async Task ResetUserPasswordAsync_InvalidNewPassword_ShouldReturnStatusCodeBadRequest(ResetPasswordRequest requestInvalidNewPassword)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestInvalidNewPassword);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:DifferentPasswords")]
    public async Task ResetUserPasswordAsync_DifferentPasswords_ShouldReturnStatusCodeBadRequest(ResetPasswordRequest requestDifferentPasswords)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestDifferentPasswords);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.PasswordDifferent);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:UserTokenNotFound")]
    public async Task ResetUserPasswordAsync_UserTokenNotFound_ShouldReturnStatusCodeBadRequest(ResetPasswordRequest requestUserTokenNotFound)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserTokenNotFound);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == UserToken.UserTokenNotFound);
    }

    [Theory, AutoUserData]
    [Trait("Test", "ResetUserPasswordAsync:UserTokenNotValidated")]
    public async Task ResetUserPasswordAsync_UserTokenNotValidated_ShouldReturnStatusCodeBadRequest(ResetPasswordRequest requestUserTokenNotValidated)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserTokenNotValidated);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/reset-password", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == UserToken.UserTokenNotValidated);
    }
    #endregion

    #region GetAccessToken
    private async Task<string> GetManagerAccessToken()
        => await GetAccessToken(UserFixture.GetManagerAuthenticateRequest());

    private async Task<string> GetCustomerAccessToken()
        => await GetAccessToken(UserFixture.GetCustomerAuthenticateRequest());

    private async Task<string> GetAccessToken(OauthRequest request)
    {
        var content = _fixture.MapObjectToStringContent(request);
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        UserResponse user = JsonConvert.DeserializeObject<UserResponse>(responseBody) ?? default!;

        return user.AccessToken;
    }
    #endregion
}