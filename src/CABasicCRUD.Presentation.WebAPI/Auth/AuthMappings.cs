using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Presentation.WebAPI.Auth.Contracts;

namespace CABasicCRUD.Presentation.WebAPI.Auth;

public static class AuthMappings
{
    public static AuthResponse ToAuthResponse(this UserResult userResult)
    {
        return new(userResult.Id, userResult.Name, userResult.Email);
    }

    public static LoginUserResponse ToLoginUserResponse(this LoginUserResult loginUserResult)
    {
        return new(loginUserResult.Id, loginUserResult.Name, loginUserResult.Email);
    }
}
