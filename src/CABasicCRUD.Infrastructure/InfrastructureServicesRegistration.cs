using System.Text.Json;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Services;
using CABasicCRUD.Infrastructure.Authentication;
using CABasicCRUD.Infrastructure.Caching;
using CABasicCRUD.Infrastructure.EmailService;
using CABasicCRUD.Infrastructure.Serialization;
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
        services.RegisterSerializer();
        services.RegisterCachingServices();

        return services;
    }

    public static IServiceCollection RegisterAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
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

    public static IServiceCollection RegisterSerializer(this IServiceCollection services)
    {
        services.AddSingleton<JsonSerializerOptions>(sp =>
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new UserIdJsonConverter());
            options.Converters.Add(new PostIdJsonConverter());
            return options;
        });

        return services;
    }

    public static IServiceCollection RegisterCachingServices(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddDistributedMemoryCache();

        services.AddSingleton<ICacheService, DistributedCacheService>();

        return services;
    }
}
