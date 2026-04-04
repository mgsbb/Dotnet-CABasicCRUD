using System.Text.Json.Serialization;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Presentation.WebApi.Common.Security;
using CABasicCRUD.Presentation.WebApi.Common.Security.Authorization;
using CABasicCRUD.Presentation.WebApi.Middlewares;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CABasicCRUD.Presentation.WebApi;

public static class PresentationServicesRegistration
{
    public static IServiceCollection RegisterPresentationServices(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            );

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

        services.AddEndpointsApiExplorer();

        services.ConfigureInvalidModelState();

        return services;
    }

    public static IServiceCollection ConfigureInvalidModelState(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var factory =
                    context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                var modelState = new ModelStateDictionary();

                var value = context.ModelState;

                foreach (var entry in context.ModelState.Where(x => x.Value?.Errors.Count > 0))
                {
                    foreach (var error in entry.Value!.Errors)
                    {
                        string errorMessage = "";

                        if (error.ErrorMessage.Contains("The JSON value could not be converted"))
                        {
                            errorMessage = "Invalid value.";
                        }
                        else if (
                            error.ErrorMessage.Contains("property name")
                            || error.ErrorMessage.Contains("invalid start of a value")
                            || error.ErrorMessage.Contains("invalid after a value")
                            || error.ErrorMessage.Contains("trailing comma")
                            || error.ErrorMessage.Contains("invalid end of a number")
                        )
                        {
                            errorMessage = "Invalid JSON format.";
                        }
                        else
                        {
                            errorMessage = error.ErrorMessage;
                        }

                        modelState.AddModelError(entry.Key, errorMessage);
                    }
                }

                var validation = factory.CreateValidationProblemDetails(
                    context.HttpContext,
                    modelState
                );

                return new BadRequestObjectResult(validation);
            };
        });

        return services;
    }
}
