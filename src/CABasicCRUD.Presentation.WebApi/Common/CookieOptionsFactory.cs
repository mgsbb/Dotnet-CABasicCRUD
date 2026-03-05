namespace CABasicCRUD.Presentation.WebApi.Common;

public static class CookieOptionsFactory
{
    public static CookieOptions CreateAccessTokenCookieOptions(IConfiguration configuration)
    {
        var expiryInMinutes = int.Parse(configuration["Jwt:ExpiryInMinutes"]!);

        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            MaxAge = TimeSpan.FromMinutes(expiryInMinutes),
        };
    }
}
