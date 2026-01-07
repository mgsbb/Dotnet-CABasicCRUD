using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace CABasicCRUD.Presentation.WebApi.RateLimiter;

internal static class RateLimitPolicies
{
    public const string Authenticated = "Authenticated";
    public const string Anonymous = "Anonymous";

    public static void Add(RateLimiterOptions options)
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddPolicy(
            Authenticated,
            context =>
            {
                var userId =
                    context.User?.Identity?.IsAuthenticated == true
                        ? context.User?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                        : null;

                var key = userId ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                // separate token buckets for every user with token
                // shared token bucket for users with the same ip (may be problematic?)
                // shared fixed window for anonymous users (is this ever the case?)
                return key != "anonymous"
                    ? GetAuthenticatedTokenBucket(key)
                    : GetAnonymousFixedWindow(key);
            }
        );

        options.AddPolicy(
            Anonymous,
            context =>
            {
                var key = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                return GetAnonymousFixedWindow(key);
            }
        );
    }

    private static RateLimitPartition<string> GetAuthenticatedTokenBucket(string key)
    {
        return RateLimitPartition.GetTokenBucketLimiter(
            key,
            _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 120,
                TokensPerPeriod = 30,
                ReplenishmentPeriod = TimeSpan.FromSeconds(60),
                AutoReplenishment = true,
                QueueLimit = 0,
            }
        );
    }

    private static RateLimitPartition<string> GetAnonymousFixedWindow(string key)
    {
        return RateLimitPartition.GetFixedWindowLimiter(
            key,
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromSeconds(60),
                AutoReplenishment = true,
                QueueLimit = 0,
            }
        );
    }
}
