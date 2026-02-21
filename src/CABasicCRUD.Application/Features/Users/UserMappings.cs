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
            UpdatedAt: user.UpdatedAt,
            Username: user.Username
        );
    }

    internal static IReadOnlyList<UserResult> ToListUserResult(this IReadOnlyList<User> users)
    {
        if (users == null)
            return new List<UserResult>();

        return users.Select(user => user.ToUserResult()).ToList();
    }
}
