using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;

namespace CABasicCRUD.Presentation.WebApi.Common.Security;

internal sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string? name, JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            // extract token from http only cookie
            OnMessageReceived = context =>
            {
                // check Authorization header (for Bearer {access_token})
                string? header = context.Request.Headers.Authorization.FirstOrDefault();
                if (!string.IsNullOrEmpty(header))
                {
                    return Task.CompletedTask;
                }

                // if not found above, extract from cookie
                context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            },

            // write custom problem details when token not found
            OnChallenge = async context =>
            {
                context.HandleResponse();

                ProblemDetailsFactory factory =
                    context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                ProblemDetails problem = factory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: StatusCodes.Status401Unauthorized,
                    detail: "Authentication token is missing or invalid."
                );

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            },
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}
