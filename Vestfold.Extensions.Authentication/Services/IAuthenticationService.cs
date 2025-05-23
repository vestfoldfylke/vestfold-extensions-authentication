using Azure.Core;
using Microsoft.Graph;

namespace Vestfold.Extensions.Authentication.Services;

public interface IAuthenticationService
{
    GraphServiceClient CreateGraphClient(string? graphBaseUrl = null, string[]? graphScopes = null);
    Task<AccessToken> GetAccessToken(string[] scopes, bool enableLogging = false);
}