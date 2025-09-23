using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.Application;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.IntegrationTests.AutoCustomData;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace EM.Authentication.IntegrationTests.EndpointsTests;

[Collection(nameof(SharedTestCollection))]
public sealed class OauthEndpointsTests
{
    private readonly HttpClient _client;

    public OauthEndpointsTests(IntegrationTestWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateAsync:ValidUser")]
    public async Task AuthenticateAsync_ValidUser_ShouldReturnStatusCodeOk(CredentialsRequest request)
    {
        //Arrange
        var content = Mapper.MapObjectToStringContent(request);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateAsync:DefaultValues")]
    public async Task AuthenticateAsync_DefaultValues_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestDefaultValues)
    {
        //Arrange
        var content = Mapper.MapObjectToStringContent(requestDefaultValues);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await Mapper.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateAsync:NullValues")]
    public async Task AuthenticateAsync_NullValues_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestNullValues)
    {
        //Arrange
        var content = Mapper.MapObjectToStringContent(requestNullValues);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await Mapper.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateAsync:UserNotFound")]
    public async Task AuthenticateAsync_UserNotFound_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestUserNotFound)
    {
        //Arrange
        var content = Mapper.MapObjectToStringContent(requestUserNotFound);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await Mapper.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    [Trait("Test", "AuthenticateAsync:IncorrectPassword")]
    public async Task AuthenticateAsync_IncorrectPassword_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestIncorrectPassword)
    {
        //Arrange
        var content = Mapper.MapObjectToStringContent(requestIncorrectPassword);

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);

        //Assert
        var errors = await Mapper.MapHttpResponseMessageToErrors(response);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidUser_ShouldReturnStatusCodeOk()
    {
        //Arrange
        var userResponse = await GetUserAuthentication();
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {userResponse!.RefreshToken}");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth/refresh-token", null);

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RefreshTokenAsync_WithoutAccessToken_ShouldReturnStatusCodeUnauthorized()
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth/refresh-token", null);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory, AutoUserData]
    public async Task RefreshTokenAsync_InvalidAccessToken_ShouldReturnStatusCodeUnauthorized(string invalidAccessToken)
    {
        //Arrange
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {invalidAccessToken}");

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth/refresh-token", null);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<UserResponse?> GetUserAuthentication()
    {
        var request = new CredentialsRequest
        {
            EmailAddress = "user@manager.com",
            Password = "123456Abc*"
        };

        var content = Mapper.MapObjectToStringContent(request);
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<UserResponse>(responseBody);
    }
}
