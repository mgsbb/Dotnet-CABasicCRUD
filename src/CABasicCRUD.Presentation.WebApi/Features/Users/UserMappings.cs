using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Users;

public static class UserMappings
{
    public static UserResponse ToUserResponse(this UserResult userResult)
    {
        return new(
            userResult.Id,
            userResult.Name,
            userResult.Email,
            userResult.Username,
            userResult.CreatedAt,
            userResult.UpdatedAt,
            userResult.Bio,
            userResult.ProfileImageUrl,
            userResult.CoverImageUrl
        );
    }

    public static IReadOnlyList<UserResponse> ToUserResponse(
        this IReadOnlyList<UserResult> userResults
    )
    {
        return userResults.Select(result => result.ToUserResponse()).ToList();
    }
}
