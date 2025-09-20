using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.Domain;
using EM.Authentication.Domain.ValueObjects;
using EM.Authentication.IntegrationTests.AutoCustomData;
using EM.Authentication.IntegrationTests.Fixtures;
using FluentAssertions;
using System.Net;
using Xunit;

namespace EM.Authentication.IntegrationTests;

[Collection(nameof(UserCollection))]
public sealed class OauthEndpointsTests : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly UserFixture _fixture;

    public OauthEndpointsTests(
        IntegrationTestWebAppFactory factory,
        UserFixture fixture)
    {
        _client = factory.CreateClient();
        _fixture = fixture;
    }

    [Theory, AutoUserData]
    [Trait("Test", "OauthAsync:ValidUser")]
    public async Task OauthAsync_ValidUser_ShouldReturnStatusCodeOk(OauthRequest request)
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
    [Trait("Test", "OauthAsync:DefaultValues")]
    public async Task OauthAsync_DefaultValues_ShouldReturnStatusCodeBadRequest(OauthRequest requestDefaultValues)
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
    [Trait("Test", "OauthAsync:NullValues")]
    public async Task OauthAsync_NullValues_ShouldReturnStatusCodeBadRequest(OauthRequest requestNullValues)
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
    [Trait("Test", "OauthAsync:UserNotFound")]
    public async Task OauthAsync_UserNotFound_ShouldReturnStatusCodeBadRequest(OauthRequest requestUserNotFound)
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
    [Trait("Test", "OauthAsync:IncorrectPassword")]
    public async Task OauthAsync_IncorrectPassword_ShouldReturnStatusCodeBadRequest(OauthRequest requestIncorrectPassword)
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
}
