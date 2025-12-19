using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Users.Errors;

public static class UserErrors
{
    public static readonly Error AlreadyExists = new(
        "User.AlreadyExists",
        "User with the given email already exists."
    );

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "Given email or password found to be incorrect"
    );
}
