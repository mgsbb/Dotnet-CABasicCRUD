using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Auth.Mapping;

internal static class AuthMappings
{
    internal static UserResult ToUserResult(this User user)
    {
        return new(Id: user.Id, Name: user.Name, Email: user.Email);
    }
}
