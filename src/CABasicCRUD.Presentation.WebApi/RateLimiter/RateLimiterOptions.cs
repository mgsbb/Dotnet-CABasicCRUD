using Microsoft.AspNetCore.RateLimiting;

namespace CABasicCRUD.Presentation.WebApi.RateLimiter;

internal static class RateLimitPolicies
{
    public static void Add(RateLimiterOptions options)
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        // TODO: scope rate limits to a particular user

        options.AddFixedWindowLimiter(
            "fixed",
            options =>
            {
                options.Window = TimeSpan.FromSeconds(60);
                options.PermitLimit = 60;
            }
        );

        options.AddSlidingWindowLimiter(
            "sliding",
            options =>
            {
                options.Window = TimeSpan.FromSeconds(15);
                options.SegmentsPerWindow = 3;
                options.PermitLimit = 15;
            }
        );

        options.AddTokenBucketLimiter(
            "token",
            options =>
            {
                options.TokenLimit = 100;
                options.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
                options.TokensPerPeriod = 10;
            }
        );

        options.AddConcurrencyLimiter(
            "concurrency",
            options =>
            {
                options.PermitLimit = 5;
            }
        );
    }
}
