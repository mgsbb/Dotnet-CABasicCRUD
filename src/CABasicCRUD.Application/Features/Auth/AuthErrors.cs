using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Auth;

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

    public static readonly Error Forbidden = new(
        "Auth.Forbidden",
        "Requested action cannot be performed."
    );
}
