using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Infrastructure.Authentication;
using CABasicCRUD.Infrastructure.EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CABasicCRUD.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection RegisterInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.RegisterAuthenticationServices(configuration);
        services.RegisterEmailSender();

        return services;
    }

    public static IServiceCollection RegisterAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetRequiredSection(JwtOptions.SectionName))
            .ValidateOnStart();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }

    public static IServiceCollection RegisterEmailSender(this IServiceCollection services)
    {
        services.AddScoped<IEmailSender, ConsoleEmailSender>();
        return services;
    }
}
