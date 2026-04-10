using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Identity.Users;

public static class UserErrors
{
    public static readonly Error NameEmpty = new("User.Name.Empty", "User name cannot be empty");
    public static readonly Error EmailEmpty = new("User.Email.Empty", "User email cannot be empty");
    public static readonly Error PasswordEmpty = new(
        "User.PasswordEmpty.Empty",
        "User password cannot be empty"
    );
    public static readonly Error UsernameEmpty = new(
        "User.Username.Empty",
        "Username cannot be empty"
    );
    public static readonly Error ProfileImageUrlEmpty = new(
        "User.ProfileImageUrl.Empty",
        "ProfileImageUrl cannot be empty"
    );
    public static readonly Error CoverImageUrlEmpty = new(
        "User.CoverImageUrl.Empty",
        "CoverImageUrl cannot be empty"
    );
}
