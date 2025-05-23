using Microsoft.Extensions.DependencyInjection;
using Vestfold.Extensions.Authentication.Services;

namespace Vestfold.Extensions.Authentication;

public static class AuthenticationExtension
{
    public static IServiceCollection AddVestfoldAuthentication(this IServiceCollection services) =>
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
}