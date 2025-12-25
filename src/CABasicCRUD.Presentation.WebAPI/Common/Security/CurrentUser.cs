using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CABasicCRUD.Application.Common.Interfaces;

namespace CABasicCRUD.Presentation.WebAPI.Common.Security;

internal sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            string? guidString = User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (guidString is null)
                return null;
            return Guid.Parse(guidString);
        }
    }

    public string? Email => User?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
}
