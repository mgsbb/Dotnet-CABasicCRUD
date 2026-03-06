using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Domain.Identity.UserProfiles;

public sealed class UserProfile : EntityBase<UserId>
{
    public string FullName { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }

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

    internal UserProfile UpdateDetails(string? fullName, string? bio)
    {
        if (fullName is not null)
        {
            FullName = fullName;
            UpdatedAt = DateTime.UtcNow;
        }
        if (bio is not null)
        {
            Bio = bio;
            UpdatedAt = DateTime.UtcNow;
        }

        return this;
    }

    internal Result<UserProfile> UpdateProfileImageUrl(string? profileImageUrl)
    {
        if (string.IsNullOrWhiteSpace(profileImageUrl))
            return Result<UserProfile>.Failure(UserErrors.ProfileImageUrlEmpty);
        ProfileImageUrl = profileImageUrl;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}
