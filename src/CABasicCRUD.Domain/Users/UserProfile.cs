using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public sealed class UserProfile : EntityBase<UserId>
{
    public string? Bio { get; private set; }
    public string? ProfileImageUrl { get; private set; }

    private UserProfile(UserId? id, string? bio, string? profileImageUrl)
        : base(id)
    {
        Bio = bio;
        ProfileImageUrl = profileImageUrl;
    }

    internal static UserProfile Create(UserId userId, string? bio, string? profileImageUrl)
    {
        return new UserProfile(userId, bio, profileImageUrl);
    }

    internal UserProfile Update(string? bio, string? profileImageUrl)
    {
        Bio = bio;
        ProfileImageUrl = profileImageUrl;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}
