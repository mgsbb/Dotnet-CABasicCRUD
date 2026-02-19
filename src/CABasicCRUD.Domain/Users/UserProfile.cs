using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public sealed class UserProfile(UserId? id, string? bio, string? profileImageUrl)
    : EntityBase<UserId>(id)
{
    public string? Bio { get; private set; } = bio;
    public string? ProfileImageUrl { get; private set; } = profileImageUrl;

    public static Result<UserProfile> Create(UserId userId, string? bio, string? profileImageUrl)
    {
        return new UserProfile(userId, bio, profileImageUrl);
    }

    public Result<UserProfile> Update(string? bio, string? profileImageUrl)
    {
        Bio = bio;
        ProfileImageUrl = profileImageUrl;
        return this;
    }
}
