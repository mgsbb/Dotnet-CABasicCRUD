using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Presentation.WebAPI.Common.Security;
using CABasicCRUD.Presentation.WebAPI.Common.Security.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CABasicCRUD.Presentation.WebAPI;

public static class PresentationServicesRegistration
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services.RegisterAuthorizationPolicies();

        return services;
    }
}
