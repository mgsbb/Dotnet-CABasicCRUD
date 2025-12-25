using CABasicCRUD.Application.Features.Users;
using CABasicCRUD.Presentation.WebAPI.Features.Users.Contracts;

namespace CABasicCRUD.Presentation.WebAPI.Features.Users;

public static class UserMappings
{
    public static UserResponse ToUserResponse(this UserResult userResult)
    {
        return new(userResult.Id, userResult.Name, userResult.Email);
    }
}
