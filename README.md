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
    "GRAPH_BASE_URL": "https://graph.microsoft.com/v1.0"
  }
}
```

## Usage outside Azure

To set `Graph base url` at a global level (optional, defaults to https://graph.microsoft.com/v1.0), add the following to your `appsettings.json` file:

```json
{
  "GRAPH_BASE_URL": "https://graph.microsoft.com/v1.0"
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
