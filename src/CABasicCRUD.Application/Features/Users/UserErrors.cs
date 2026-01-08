using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Users;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "User does not exist.");

    public static readonly Error NotOwner = new(
        "User.NotOwner",
        "User does not belong to the current user."
    );
}
