using EM.Authentication.API.Users.RequestModels;
using EM.Authentication.Application;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using EM.Authentication.Domain.ValueObjects;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using WH.SharedKernel.ResourceManagers;
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
            fixture.AccessToken = GetAccessToken().Result;

        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {fixture.AccessToken}");
    }

    [Theory, AutoUserData]
    public async Task AddCustomerAsync_ValidCustomer_ShouldReturnStatusCodeOk(AddCustomerRequest request)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users/customer-profile", MapObjectToStringContent(request));

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_ValidUser_ShouldReturnStatusCodeOk(AddUserRequest request)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(request));

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_WithoutAccessToken_ShouldReturnStatusCodeUnauthorized(AddUserRequest request)
    {
        //Arrange
        _client.DefaultRequestHeaders.Clear();

        //Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(request));

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AddUserAsync_DefaultValues_ShouldReturnStatusCodeBadRequest()
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(new AddUserRequest()));

        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.UserNameNullOrEmpty);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
        errors.Should().Contain(x => x.Message == Profile.ProfileNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_NullValues_ShouldReturnStatusCodeBadRequest(AddUserRequest nullValues)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(nullValues));
        
        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.UserNameNullOrEmpty);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
        errors.Should().Contain(x => x.Message == Profile.ProfileNameNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_UserNameAndEmailAddressLargerThanMaxLenght_ShouldReturnStatusCodeBadRequest(AddUserRequest largerThanMaxLenght)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(largerThanMaxLenght));

        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.UserNameMaxLenghtError);
        errors.Should().Contain(x => x.Message == Email.EmailAddressMaxLenghtError);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_InvalidUserNameAndEmailAddress_ShouldReturnFailure(AddUserRequest invalidValues)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(invalidValues));

        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.InvalidEmailAddress);
        errors.Should().Contain(x => x.Message == User.InvalidPassword);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_DifferentPasswords_ShouldReturnFailure(AddUserRequest differentPasswords)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(differentPasswords));
        
        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.PasswordDifferent);
    }

    [Theory, AutoUserData]
    public async Task AddUserAsync_ProfileNameNotFound_ShouldReturnFailure(AddUserRequest profileNameNotFound)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/users", MapObjectToStringContent(profileNameNotFound));
        
        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Profile.ProfileNotFound);
    }

    [Theory, AutoUserData]
    public async Task AuthenticateUserAsync_ValidUser_ShouldReturnStatusCodeOk(AuthenticateUserRequest request)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", MapObjectToStringContent(request));

        //Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AuthenticateUserAsync_DefaultValues_ShouldReturnStatusCodeBadRequest()
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", MapObjectToStringContent(new AuthenticateUserRequest()));
        
        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task AuthenticateUserAsync_NullValues_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest nullValues)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", MapObjectToStringContent(nullValues));
        
        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == Email.EmailAddressNullOrEmpty);
        errors.Should().Contain(x => x.Message == User.PasswordNullOrEmpty);
    }

    [Theory, AutoUserData]
    public async Task AuthenticateUserAsync_EmailAddressNotFound_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest emailAddressNotFound)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", MapObjectToStringContent(emailAddressNotFound));

        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    [Theory, AutoUserData]
    public async Task AuthenticateUserAsync_IncorrectPassword_ShouldReturnStatusCodeBadRequest(AuthenticateUserRequest incorrectPassword)
    {
        //Arrange & Act
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", MapObjectToStringContent(incorrectPassword));

        var errors = await MapHttpResponseMessageToErrors(response);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        errors.Should().Contain(x => x.Message == User.EmailAddressOrPasswordIncorrect);
    }

    private async Task<string> GetAccessToken()
    {
        var request = _fixture.GetValidAuthenticateUserRequest();

        HttpResponseMessage response = await _client.PostAsync("/api/oauth", MapObjectToStringContent(request));
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        UserResponse user = JsonConvert.DeserializeObject<UserResponse>(responseBody) ?? default!;
        
        return user.AccessToken;
    }

    private StringContent MapObjectToStringContent(object request) =>
        new(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

    private async Task<IEnumerable<Error>?> MapHttpResponseMessageToErrors(HttpResponseMessage response)
    {
        string responseString = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<IEnumerable<Error>>(responseString);
    }
}