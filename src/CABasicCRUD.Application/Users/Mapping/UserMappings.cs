using CABasicCRUD.Application.Users.DTOs;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Users.Mapping;

internal static class UserMappings
{
    internal static UserResult ToUserResult(this User user)
    {
        return new(Id: user.Id, Name: user.Name, Email: user.Email);
    }
}
