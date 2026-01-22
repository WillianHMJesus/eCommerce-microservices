using EM.Authentication.BehaviorTests.Contexts.Customers;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace EM.Authentication.BehaviorTests.Features.Customers;

[Binding]
public sealed class CustomerSteps
{
    private readonly HttpClient _client;
    private readonly CustomerContext _context;
    private readonly AddCustomerRequestContext _requestContext;

    public CustomerSteps(
        HttpClient client,
        CustomerContext context,
        AddCustomerRequestContext requestContext)
    {
        _client = client;
        _context = context;
        _requestContext = requestContext;
    }

    //Given
    [Given(@"I have customer registration data with {string}")]
    public void GivenIHaveCustomerRegistrationData(string parameterName)
    {
        _context.AddCustomerRequest = _requestContext.GetRequest(parameterName);
    }

    //When
    [When(@"I register the customer")]
    public async Task WhenIRegisterTheCustomer()
    {
        var json = JsonConvert.SerializeObject(_context.AddCustomerRequest);

        var response = await _client.PostAsync(
            "/api/users/customer-profile",
            new StringContent(json, Encoding.UTF8, "application/json"));

        _context.Response = response;
    }

    //Then
    [Then(@"The new customer must be registered")]
    public void ThenTheNewCustomerMustBeRegistered()
    {
        var response = _context.Response ?? throw new ArgumentNullException();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"the customer registration must be rejected due to validation errors")]
    public async Task ThenTheCustomerRegistrationMustBeRejected(Table table)
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
