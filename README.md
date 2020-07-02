## ReqHubDotNet
ReqHub middleware for C# projects. Distribute your API using the ReqHub platform in just a few lines!
For more information, visit https://reqhub.io.

## Distributing an API
To distribute an API for clients to consume with API keys, add these two lines to your `Startup.cs`:

```cs
/// Startup.cs

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Visit https://reqhub.io to get your API keys
        services.AddReqHub("yourPublicKey", "yourPrivateKey");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // ...
        // app.UseRouting();
        
        // Put this after app.UseRouting() and before app.UseEndpoints().
        // It verifies requests against the platform and either
        // continues execution or returns a 403 forbidden response
        // if the request is invalid (bad API key, contents tampered with, etc.)
        app.UseReqHub();
        
        // app.UseEndpoints(...);
        // ...
    }
}
```
That's it! 🎉

#### Identity
You may want to be able to uniquely identify a client.
Inside your controller methods you can access the clientId with:
```cs
this.User.GetClientId();
```

You can also access plan information:
```cs
this.User.GetPlanName();
this.User.GetNormalizedPlanName();
this.User.GetPlanSku();
this.User.GetNormalizedPlanSku();
```

#### How it works
Clients consuming your API create a request hash using their own API keys, which the middleware forwards to the platform
along with your request hash. If everything matches up on the platform, the request is allowed to continue.

## Consuming an API
To consume an API, configure a client and inject it into your controllers/services.

```cs
/// Startup.cs

public class Startup
{
    // ...
    
    public void ConfigureServices(IServiceCollection services)
    {
        // Visit https://reqhub.io to get your API keys
        services.AddApiClient("https://api-base-address", "yourClientPublicKey", "yourClientPrivateKey", "serviceName");
        
        // Add as many as you like!
        services.AddApiClient("https://api2-base-address", "anotherClientPublicKey", "anotherClientPrivateKey", "serviceName2");
    }
}
```
(it looks like a lot of code, but we promise it isn't 👌):
```cs
/// ExampleController.cs

public class ExampleController : ControllerBase
{
    private readonly IApiClient exampleApiClient;

    // Resolve the API client using dependency injection
    public ExampleController(ApiClientResolver apiClientResolver)
    {
        this.exampleApiClient = apiClientResolver("serviceName");
    }

    [HttpGet]
    public async Task<IEnumerable<Example>> Get()
    {
        return await this.exampleApiClient.GetAsync<IEnumerable<Example>>("/example/endpoint");
    }
}
```

## .NET Framework
For those of you not using .NET Core, ReqHub supports .NET Framework (4.5+) WebApi projects.

```cs
/// FilterConfig.cs

public class FilterConfig
{
    public static void RegisterGlobalFilters(HttpFilterCollection filters)
    {
        // ...
        filters.Add(new ReqHubAttribute("yourPublicKey", "yourPrivateKey"));
    }
}
```

To consume an API:

```cs
// A little bit of setup
// (note that the HttpClient should be instantiated once and reused throughout the life of the application https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netcore-3.1#remarks)
var httpClient = HttpClientFactory.Create(new ReqHubClientHttpMessageHandler("yourClientPublicKey", "yourClientPrivateKey"));
httpClient.BaseAddress = new Uri("https://api-base-address");
var exampleApiClient = new ApiClient(httpClient);

await this.exampleApiClient.GetAsync<IEnumerable<Example>>("/example/endpoint");
```

## Contributing
Go for it! If we're missing something or you're running into a problem, either let us know in an issue or send us a pull request.
We think we're pretty reasonable 😘

## License
MIT, babe -- go nuts! 🎉
