namespace CABasicCRUD.Presentation.WebApi.Features.Auth.Contracts;

public sealed record AuthResponse(
    Guid Id,
    string Name,
    string Email,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
