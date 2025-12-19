using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public static class UserErrors
{
    public static readonly Error NameEmpty = new("User.Name.Empty", "User name cannot be empty");
    public static readonly Error EmailEmpty = new("User.Email.Empty", "User email cannot be empty");
    public static readonly Error PasswordEmpty = new(
        "User.PasswordEmpty.Empty",
        "User password cannot be empty"
    );
}
