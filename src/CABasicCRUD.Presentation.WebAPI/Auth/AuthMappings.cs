using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Presentation.WebAPI.Auth.Contracts;

namespace CABasicCRUD.Presentation.WebAPI.Auth;

public static class AuthMappings
{
    public static AuthResponse ToAuthResponse(this AuthResult authResult)
    {
        return new(authResult.Id, authResult.Name, authResult.Email);
    }
}
