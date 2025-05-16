using Microsoft.Extensions.DependencyInjection;
using VFK.Extensions.Authentication.Services;

namespace VFK.Extensions.Authentication;

public static class AuthenticationExtension
{
    public static IServiceCollection AddVfkAuthentication(this IServiceCollection services) =>
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
}