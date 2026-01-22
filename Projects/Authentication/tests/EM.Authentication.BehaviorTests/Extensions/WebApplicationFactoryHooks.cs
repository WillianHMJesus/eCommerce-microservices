using Reqnroll.BoDi;

namespace EM.Authentication.BehaviorTests.Extensions;

[Binding]
public sealed class WebApplicationFactoryHooks
{
    private static BehaviorTestWebAppFactory? _factory;
    private static HttpClient? _client;

    private readonly IObjectContainer _container;

    public WebApplicationFactoryHooks(IObjectContainer container)
    {
        _container = container;
    }

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _factory = new BehaviorTestWebAppFactory();
        await _factory.InitializeAsync();
        _client = _factory.CreateClient();
    }

    [BeforeScenario]
    public void RegisterHttpClient()
    {
        _container.RegisterInstanceAs(_client!);
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (_factory is not null)
        {
            await _factory.DisposeAsync();
        }
    }
}

