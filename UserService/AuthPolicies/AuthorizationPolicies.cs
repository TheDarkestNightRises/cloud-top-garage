using System.Security.Claims;

namespace UserService.AuthPolicies;
public static class AuthorizationPolicies
{
    public static void AddPolicies(IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("MustBeUser", a =>
                a.RequireAuthenticatedUser().RequireClaim("Role", "User"));
        });
    }
}
