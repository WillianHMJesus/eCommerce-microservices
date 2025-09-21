using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.IntegrationTests.AutoCustomData;
using FluentAssertions;
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
    [Trait("Test", "OauthAsync:ValidUser")]
    public async Task OauthAsync_ValidUser_ShouldReturnStatusCodeOk(CredentialsRequest request)
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
    [Trait("Test", "OauthAsync:DefaultValues")]
    public async Task OauthAsync_DefaultValues_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestDefaultValues)
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
    [Trait("Test", "OauthAsync:NullValues")]
    public async Task OauthAsync_NullValues_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestNullValues)
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
    [Trait("Test", "OauthAsync:UserNotFound")]
    public async Task OauthAsync_UserNotFound_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestUserNotFound)
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
    [Trait("Test", "OauthAsync:IncorrectPassword")]
    public async Task OauthAsync_IncorrectPassword_ShouldReturnStatusCodeBadRequest(CredentialsRequest requestIncorrectPassword)
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
}
