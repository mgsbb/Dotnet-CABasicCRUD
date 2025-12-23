using System.Text;
using CABasicCRUD.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CABasicCRUD.Presentation.WebAPI.Common;

internal sealed class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public void Configure(string? name, JwtBearerOptions options)
    {
        // Important - to correctly receive JwtRegisteredClaimNames.Sub and JwtRegisteredClaimNames.Email in CurrentUser
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)
            ),
        };

        // extract token from http only cookie
        options.Events = new JwtBearerEvents
        {
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
        };
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }
}
