using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Presentation.WebApi.Features.Auth.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Auth;

public static class AuthMappings
{
    public static AuthResponse ToAuthResponse(this AuthResult authResult)
    {
        return new(authResult.Id, authResult.Name, authResult.Email);
    }
}
