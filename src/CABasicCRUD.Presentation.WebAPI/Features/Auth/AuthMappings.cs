using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Presentation.WebAPI.Features.Auth.Contracts;

namespace CABasicCRUD.Presentation.WebAPI.Features.Auth;

public static class AuthMappings
{
    public static AuthResponse ToAuthResponse(this AuthResult authResult)
    {
        return new(authResult.Id, authResult.Name, authResult.Email);
    }
}
