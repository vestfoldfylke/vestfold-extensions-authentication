using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Diagnostics;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace Vestfold.Extensions.Authentication.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly TokenCredential _tokenCredential;

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

        var clientSecretOptions = new ClientSecretCredentialOptions
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

        var clientId = configuration["AZURE_CLIENT_ID"];
        var clientSecret = configuration["AZURE_CLIENT_SECRET"];
        var tenantId = configuration["AZURE_TENANT_ID"];

        _tokenCredential = string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(tenantId)
            ? new DefaultAzureCredential(defaultAzureOptions)
            : new ClientSecretCredential(tenantId, clientId, clientSecret, clientSecretOptions);
    }

    public GraphServiceClient CreateGraphClient(string? graphBaseUrl = null, string[]? graphScopes = null)
    {
        graphBaseUrl ??= _graphBaseUrl;

        graphScopes ??=
        [
            DefaultGraphScope
        ];

        return new GraphServiceClient(_tokenCredential, graphScopes, graphBaseUrl);
    }

    public async Task<AccessToken> GetAccessToken(string[] scopes, bool enableLogging = false, CancellationToken cancellationToken = default)
    {
        if (!enableLogging)
        {
            return await _tokenCredential.GetTokenAsync(new TokenRequestContext(scopes), cancellationToken);
        }

        using var listener = AzureEventSourceListener.CreateConsoleLogger();
        return await _tokenCredential.GetTokenAsync(new TokenRequestContext(scopes), cancellationToken);
    }
}