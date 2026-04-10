namespace CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? Bio,
    string? ProfileImageUrl,
    string? CoverImageUrl
);
