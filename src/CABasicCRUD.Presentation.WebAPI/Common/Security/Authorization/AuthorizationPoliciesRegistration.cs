using CABasicCRUD.Presentation.WebApi.Common.Security.Authorization.ResourceOwner;
using Microsoft.AspNetCore.Authorization;

namespace CABasicCRUD.Presentation.WebApi.Common.Security.Authorization;

public static class AuthorizationPoliciesRegistration
{
    public static IServiceCollection RegisterAuthorizationPolicies(this IServiceCollection services)
    {
        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy(
        //         AuthorizationPolicies.ResourceOwner,
        //         policy => policy.Requirements.Add(new ResourceOwnerRequirement())
        //     );
        // });

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                AuthorizationPolicies.ResourceOwner,
                policy => policy.Requirements.Add(new ResourceOwnerRequirement())
            );

        services.AddScoped<IAuthorizationHandler, ResourceOwnerHandler>();

        return services;
    }
}
