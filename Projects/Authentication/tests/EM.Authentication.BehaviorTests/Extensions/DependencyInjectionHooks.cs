using EM.Authentication.BehaviorTests.Contexts.Authentication;
using EM.Authentication.BehaviorTests.Contexts.Customers;
using EM.Authentication.BehaviorTests.Contexts.Users;
using Reqnroll.BoDi;

namespace EM.Authentication.BehaviorTests.Extensions;

[Binding]
public class DependencyInjectionHooks
{
    private readonly IObjectContainer _objectContainer;

    public DependencyInjectionHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario]
    public void RegisterDependencies()
    {
        //Customer
        _objectContainer.RegisterTypeAs<CustomerContext, CustomerContext>();
        _objectContainer.RegisterTypeAs<AddCustomerRequestContext, AddCustomerRequestContext>();

        //User
        _objectContainer.RegisterTypeAs<UserContext, UserContext>();
        _objectContainer.RegisterTypeAs<AddUserRequestContext, AddUserRequestContext>();
        _objectContainer.RegisterTypeAs<ChangeUserPasswordRequestContext, ChangeUserPasswordRequestContext>();

        //Authentication
        _objectContainer.RegisterTypeAs<CredentialsRequestContext, CredentialsRequestContext>();
    }
}
