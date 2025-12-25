using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Infrastructure.Authentication;
using CABasicCRUD.Presentation.WebAPI.Common.Options;
using CABasicCRUD.Presentation.WebAPI.Common.Security;
using CABasicCRUD.Presentation.WebAPI.Common.Security.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CABasicCRUD.Presentation.WebAPI;

public static class PresentationServicesRegistration
{
    public static IServiceCollection RegisterPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddControllers();

        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.ConfigureOptions<JwtBearerOptionsSetup>();
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
