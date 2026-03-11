namespace CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;

public sealed record UpdateUserPasswordRequest(
    string OldPassword,
    string NewPassword,
    string NewPasswordConfirmed
);
