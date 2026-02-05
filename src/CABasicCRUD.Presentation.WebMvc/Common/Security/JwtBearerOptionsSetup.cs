using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace CABasicCRUD.Presentation.WebMvc.Common.Security;

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

            // redirect if token not found
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.Redirect("/auth/login");

                await Task.CompletedTask;
            },
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}
