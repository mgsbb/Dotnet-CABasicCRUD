using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Presentation.WebApi.Common.Security;
using CABasicCRUD.Presentation.WebApi.Common.Security.Authorization;
using CABasicCRUD.Presentation.WebApi.Middlewares;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CABasicCRUD.Presentation.WebApi;

public static class PresentationServicesRegistration
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        services.RegisterAuthorizationPolicies();

        services.AddRateLimiter(RateLimitPolicies.Add);

        return services;
    }
}
