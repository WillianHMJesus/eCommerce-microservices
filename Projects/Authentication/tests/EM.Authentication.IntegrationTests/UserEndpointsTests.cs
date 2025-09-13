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

    #region AuthenticateUser
    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUserAsync:ValidUser")]
    public async Task AuthenticateUserAsync_ValidUser_ShouldReturnStatusCodeOk(AuthenticateUserRequest request)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUserAsync:DefaultValues")]
    public async Task AuthenticateUserAsync_DefaultValues_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest requestDefaultValues)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestDefaultValues);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUserAsync:NullValues")]
    public async Task AuthenticateUserAsync_NullValues_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest requestNullValues)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestNullValues);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUserAsync:UserNotFound")]
    public async Task AuthenticateUserAsync_UserNotFound_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest requestUserNotFound)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestUserNotFound);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateUserAsync:IncorrectPassword")]
    public async Task AuthenticateUserAsync_IncorrectPassword_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest requestIncorrectPassword)
    {
        //Arrange
        var content = _fixture.MapObjectToStringContent(requestIncorrectPassword);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await _fixture.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
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

    #region GetAccessToken
    private async Task<string> GetManagerAccessToken()
        => await GetAccessToken(UserFixture.GetManagerAuthenticateRequest());

    private async Task<string> GetCustomerAccessToken()
        => await GetAccessToken(UserFixture.GetCustomerAuthenticateRequest());

    private async Task<string> GetAccessToken(AuthenticateUserRequest request)
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