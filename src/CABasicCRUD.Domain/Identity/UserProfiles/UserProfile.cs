using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;

namespace CABasicCRUD.Domain.Identity.UserProfiles;

public sealed class UserProfile : EntityBase<UserId>
{
    public string FullName { get; set; }
    public string? Bio { get; set; }

    public MediaId? ProfileImageId { get; set; }
    public MediaId? CoverImageId { get; set; }

    private UserProfile(
        UserId? id,
        string fullName,
        string? bio,
        MediaId? profileImageId,
        MediaId? coverImageId
    )
        : base(id)
    {
        FullName = fullName;
        Bio = bio;
        ProfileImageId = profileImageId;
        CoverImageId = coverImageId;
    }

    internal static UserProfile Create(
        UserId userId,
        string fullName,
        string? bio,
        MediaId? profileImageId,
        MediaId? coverImageId
    )
    {
        return new UserProfile(userId, fullName, bio, profileImageId, coverImageId);
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

    internal Result<UserProfile> UpdateProfileImageId(MediaId profileImageId)
    {
        ProfileImageId = profileImageId;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    internal Result<UserProfile> UpdateCoverImageId(MediaId coverImageId)
    {
        CoverImageId = coverImageId;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }
}
