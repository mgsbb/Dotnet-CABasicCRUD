using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Auth.Errors;

public static class AuthErrors
{
    public static readonly Error AlreadyExists = new(
        "Auth.AlreadyExists",
        "User with the given email already exists."
    );

    public static readonly Error InvalidCredentials = new(
        "Auth.InvalidCredentials",
        "Given email or password found to be incorrect."
    );
}
