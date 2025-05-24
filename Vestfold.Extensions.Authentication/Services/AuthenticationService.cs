using Azure.Core;
using Azure.Core.Diagnostics;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace Vestfold.Extensions.Authentication.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly DefaultAzureCredential _azureCredential;

    private readonly string _graphBaseUrl;
    
    private const string DefaultGraphBaseUrl = "https://graph.microsoft.com/v1.0";
    private const string DefaultGraphScope = "https://graph.microsoft.com/.default";
    
    public AuthenticationService(IConfiguration configuration)
    {
        var defaultAzureOptions = new DefaultAzureCredentialOptions
        {
            Diagnostics =
            {
                IsLoggingEnabled = true,
                IsLoggingContentEnabled = true,
                LoggedHeaderNames = { "x-ms-request-id" },
                LoggedQueryParameters = { "api-version" },
                IsAccountIdentifierLoggingEnabled = true
            }
        };
        
        _graphBaseUrl = configuration["GRAPH_BASE_URL"] ?? DefaultGraphBaseUrl;
        
        _azureCredential = new DefaultAzureCredential(defaultAzureOptions);
    }
    
    public GraphServiceClient CreateGraphClient(string? graphBaseUrl = null, string[]? graphScopes = null)
    {
        graphBaseUrl ??= _graphBaseUrl;
        
        graphScopes ??=
        [
            DefaultGraphScope
        ];
        
        return new GraphServiceClient(_azureCredential, graphScopes, graphBaseUrl);
    }

    public async Task<AccessToken> GetAccessToken(string[] scopes, bool enableLogging = false)
    {
        if (!enableLogging)
        {
            return await _azureCredential.GetTokenAsync(new TokenRequestContext(scopes));
        }
        
        using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();
        return await _azureCredential.GetTokenAsync(new TokenRequestContext(scopes));
    }
}