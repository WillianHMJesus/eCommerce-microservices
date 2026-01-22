using EM.Authentication.API.Oauth.RequestModels;
using EM.Authentication.Application;
using EM.Authentication.BehaviorTests.Contexts.Authentication;
using EM.Authentication.BehaviorTests.Contexts.Users;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EM.Authentication.BehaviorTests.Features.Users;

[Binding]
public sealed class UserSteps
{
    private readonly HttpClient _client;
    private readonly UserContext _context;
    private readonly AddUserRequestContext _addUserRequestContext;
    private readonly ChangeUserPasswordRequestContext _changeUserPasswordRequestContext;
    private readonly CredentialsRequestContext _credentialsRequestContext;

    public UserSteps(
        HttpClient client,
        UserContext context,
        AddUserRequestContext addUserRequestContext,
        ChangeUserPasswordRequestContext changeUserPasswordRequestContext,
        CredentialsRequestContext credentialsRequestContext)
    {
        _client = client;
        _context = context;
        _addUserRequestContext = addUserRequestContext;
        _changeUserPasswordRequestContext = changeUserPasswordRequestContext;
        _credentialsRequestContext = credentialsRequestContext;
    }

    //Given
    [Given(@"I am authenticated in with the {string}")]
    public async Task GivenIAmAuthenticated(string userProfile)
    {
        CredentialsRequest request = _credentialsRequestContext.GetRequest(userProfile);
        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PostAsync("/api/oauth", content);
        
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var userResponse = JsonConvert.DeserializeObject<UserResponse>(responseBody);
        
        _context.AccessToken = userResponse?.AccessToken;
    }

    [Given(@"I have user registration data with {string}")]
    public void GivenIHaveUserRegistrationData(string parameterName)
    {
        _context.AddUserRequest = _addUserRequestContext.GetRequest(parameterName);
    }

    [Given(@"I am not authenticated")]
    public void GivenIAmNotAuthenticated()
    {
        _context.AccessToken = null;
    }

    [Given(@"I am not authenticated due to invalid credentials")]
    public void GivenIAmNotAuthenticatedDueInvalidCredentials()
    {
        _context.AccessToken = "invalid access token";
    }

    [Given(@"I have password change data for (?:my user|another user|user) with ""(.*)""")]
    public void GivenIHavePasswordChangeData(string parameterName)
    {
        _context.ChangeUserPasswordRequest = _changeUserPasswordRequestContext.GetRequest(parameterName);
    }

    //When
    [When(@"I register the user")]
    public async Task WhenIRegisterTheUser()
    {
        _client.DefaultRequestHeaders.Clear();
        var json = JsonConvert.SerializeObject(_context.AddUserRequest);

        if (_context.AccessToken is not null)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.AccessToken}");
        }

        var response = await _client.PostAsync(
            "/api/users",
            new StringContent(json, Encoding.UTF8, "application/json"));

        _context.Response = response;
    }

    [When(@"I change my password")]
    public async Task WhenIChangeMyPassword()
    {
        _client.DefaultRequestHeaders.Clear();
        var json = JsonConvert.SerializeObject(_context.ChangeUserPasswordRequest);

        if (_context.AccessToken is not null)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_context.AccessToken}");
        }

        var response = await _client.PutAsync(
            "/api/users/change-password",
            new StringContent(json, Encoding.UTF8, "application/json"));

        _context.Response = response;
    }

    //Then
    [Then(@"the new user must be registered")]
    [Then(@"the password must be changed")]
    public void ThenTheOperationMustBeSuccessful()
    {
        var response = _context.Response ?? throw new ArgumentNullException();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"the user registration must be rejected due to insufficient permissions")]
    [Then(@"the password must not be changed due to insufficient permissions")]
    public void ThenTheOperationMustBeRejectedDueInsufficientPermissions()
    {
        var response = _context.Response ?? throw new ArgumentNullException();

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Then(@"the user registration must be rejected because the user is not authenticated")]
    [Then(@"the password change must be rejected because the user is not authenticated")]
    public void ThenTheOperationMustBeRejectedDueUserNotAuthenticated()
    {
        var response = _context.Response ?? throw new ArgumentNullException();

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Then(@"the user registration must be rejected due to validation errors")]
    [Then(@"the password must not be changed due to validation errors")]
    public async Task ThenTheOperationMustBeRejected(Table table)
    {
        var response = _context.Response ?? throw new ArgumentNullException();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadAsStringAsync();

        foreach (var row in table.Rows)
        {
            content.Should().Contain(row[0]);
        }
    }
}
