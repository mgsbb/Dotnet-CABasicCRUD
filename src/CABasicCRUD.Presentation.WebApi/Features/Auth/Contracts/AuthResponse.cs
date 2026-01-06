namespace CABasicCRUD.Presentation.WebApi.Features.Auth.Contracts;

public sealed record AuthResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
