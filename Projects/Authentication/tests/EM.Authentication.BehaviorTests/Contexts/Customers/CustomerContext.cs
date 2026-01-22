using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.BehaviorTests.Contexts.Customers;

public class CustomerContext
{
    public AddCustomerRequest? AddCustomerRequest { get; set; }
    public HttpResponseMessage? Response { get; set; }
}
