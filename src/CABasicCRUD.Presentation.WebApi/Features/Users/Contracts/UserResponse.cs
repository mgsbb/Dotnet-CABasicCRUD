namespace CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
