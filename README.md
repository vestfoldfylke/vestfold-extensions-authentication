![NuGet Version](https://img.shields.io/nuget/v/Vestfold.Extensions.Authentication.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/Vestfold.Extensions.Authentication.svg)

# Vestfold.Extensions.Authentication

Contains builder extensions to extend a dotnet core application with authentication functionality.

## Usage in an Azure Function / Azure Web App

To set `Graph base url` at a global level (optional, defaults to https://graph.microsoft.com/v1.0), add the following to your `local.settings.json` file:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "GRAPH_BASE_URL": "https://graph.microsoft.com/v1.0",
    "AZURE_CLIENT_ID": "your-azure-app-registration-client-id-guid",
    "AZURE_CLIENT_SECRET": "your-azure-app-registration-client-secret",
    "AZURE_TENANT_ID": "your-azure-tenant-id-guid"
  }
}
```

## Usage outside Azure

To set `Graph base url` at a global level (optional, defaults to https://graph.microsoft.com/v1.0), add the following to your `appsettings.json` file:

```json
{
  "GRAPH_BASE_URL": "https://graph.microsoft.com/v1.0",
  "AZURE_CLIENT_ID": "your-azure-app-registration-client-id-guid",
  "AZURE_CLIENT_SECRET": "your-azure-app-registration-client-secret",
  "AZURE_TENANT_ID": "your-azure-tenant-id-guid"
}
```

## Setting up for an Azure Function / Azure Web App

```csharp
var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Services.AddVestfoldAuthentication();
builder.Build().Run();
```

## Setting up for a HostBuilder (Console app, ClassLibrary, etc.)

```csharp
public static async Task Main(string[] args)
{
    await Host.CreateDefaultBuilder(args)
        .ConfigureServices(services => services.AddVestfoldAuthentication())
        .Build()
        .RunAsync();

    await Serilog.Log.CloseAndFlushAsync();
}
```

## Setting up for a WebApplicationBuilder (WebAPI, Blazor, etc.)

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddVestfoldAuthentication();

var app = builder.Build();
```

## Using the authentication methods

To use the authentication methods, you inject the `IAuthenticationService` into your classes.

Then you call:
- `CreateGraphClient` - which will return a `Microsoft.Graph.GraphServiceClient` that can be used to call the Microsoft Graph API (with the permissions granted to the app registration (AZURE_CLIENT_ID))
- `GetAccessToken` - which will return a `Azure.Core.AccessToken` for specified scopes from the app registration (AZURE_CLIENT_ID)

```csharp
public class Something
{
    private readonly GraphServiceClient _graphClient;
    private readonly AccessToken _accessToken;
    
    public Something(IAuthenticationService authenticationService)
    {
        // you can optionally pass a base URL for the Graph API, otherwise it will use the base URL from configuration, or the default base URL https://graph.microsoft.com/v1.0
        // you can also optionally pass custom scopes, otherwise it will use the default scope https://graph.microsoft.com/.default
        _graphClient = authenticationService.CreateGraphClient();
        
        // you can optionally pass a boolean to specify if you want to enable logging for the Azure Credential token process, otherwise it will default to false
        _accessToken = await authenticationService.GetAccessToken(["https://whatever.no/.default"]);
    }
}
```