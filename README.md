## ReqHubDotNet
ReqHub middleware for C# projects. Distribute your API using the ReqHub platform in just a few lines!
For more information, visit https://reqhub.io.

## Installation
ReqHub for .Net is available on NuGet: https://www.nuget.org/packages/ReqHub
```
Install-Package ReqHub
```

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

## Plan information
Plan information can be accessed from the `User` object.
You can use this data to change or restrict functionality by plan.

```cs
this.User.GetClientId();           // A clientId unique to the user
this.User.GetPlanName();           // The plan name as entered, like "Extra awesome"
this.User.GetNormalizedPlanName(); // The plan name whitespace and special characters removed, like "Extra-awesome"
this.User.GetPlanSku();            // The plan SKU as entered, like "Extra awesome SKU!!!"
this.User.GetNormalizedPlanSku();  // The plan SKU with whitespace and special characters removed, like "Extra-awesome-SKU"
this.User.IsTrial();               // Indicates whether the user is currently in a trial period
```

Check out our docs for [testing pricing plans](https://docs.reqhub.io/#/recipes/simulating-pricing-plans) for more information.

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

## Per-endpoint configuration
ReqHub can be configured on a per-endpoint or per-controller basis. This is useful if you only want to publish a portion of your API, or if you want to include multiple small APIs in a single server instance to reduce hosting costs.

Simply apply the `ReqHub` attribute and you're all set!

```cs
// ReqHub attribute on a single action method

[HttpGet("test")]
[ReqHub(publicKey: "yourPublicKey", privateKey: "yourPrivateKey")]
public IActionResult Test()
{
    return this.Ok("Success!");
}
```

Placing the attribute at the controller level will apply to all its endpoints:
```cs
// ReqHub attribute on a controller

[Route("example")]
[ApiController]
[ReqHub(publicKey: "yourPublicKey", privateKey: "yourPrivateKey")]
public class ExampleController : ControllerBase
{
    [HttpGet("test1")]
    public IActionResult Test1()
    {
        return this.Ok("Success!");
    }
        
    [HttpGet("test2")]
    public IActionResult Test2()
    {
        return this.Ok("Success!");
    }
}
```

The examples here were for the latest .Net, but the same applies to .Net Framework projects.

## Contributing
Go for it! If we're missing something or you're running into a problem, either let us know in an issue or send us a pull request.
We think we're pretty reasonable 😘

## License
MIT, babe -- go nuts! 🎉
