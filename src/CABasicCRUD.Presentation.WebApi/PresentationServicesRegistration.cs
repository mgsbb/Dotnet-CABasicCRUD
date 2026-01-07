using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Presentation.WebApi.Common.Security;
using CABasicCRUD.Presentation.WebApi.Common.Security.Authorization;
using CABasicCRUD.Presentation.WebApi.Middlewares;
using CABasicCRUD.Presentation.WebApi.RateLimiter;

namespace CABasicCRUD.Presentation.WebApi;

public static class PresentationServicesRegistration
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        services.RegisterAuthorizationPolicies();

        services.AddRateLimiter(RateLimitPolicies.Add);

        return services;
    }
}
