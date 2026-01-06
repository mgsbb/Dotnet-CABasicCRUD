using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users;

internal static class UserMappings
{
    internal static UserResult ToUserResult(this User user)
    {
        return new(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt
        );
    }
}
