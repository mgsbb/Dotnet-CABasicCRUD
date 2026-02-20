using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public sealed class UserProfile : EntityBase<UserId>
{
    public string FullName { get; private set; }
    public string? Bio { get; private set; }
    public string? ProfileImageUrl { get; private set; }

    private UserProfile(UserId? id, string fullName, string? bio, string? profileImageUrl)
        : base(id)
    {
        FullName = fullName;
        Bio = bio;
        ProfileImageUrl = profileImageUrl;
    }

    internal static UserProfile Create(
        UserId userId,
        string fullName,
        string? bio,
        string? profileImageUrl
    )
    {
        return new UserProfile(userId, fullName, bio, profileImageUrl);
    }

    internal UserProfile Update(string? fullName, string? bio, string? profileImageUrl)
    {
        if (fullName is not null)
            FullName = fullName;
        Bio = bio;
        ProfileImageUrl = profileImageUrl;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}
