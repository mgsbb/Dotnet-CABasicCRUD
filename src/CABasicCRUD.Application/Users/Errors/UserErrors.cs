using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.NotFound",
        "User with the given email does not exist."
    );
}
