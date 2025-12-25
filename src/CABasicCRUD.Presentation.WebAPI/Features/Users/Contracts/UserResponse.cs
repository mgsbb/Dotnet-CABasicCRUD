namespace CABasicCRUD.Presentation.WebAPI.Features.Users.Contracts;

public sealed record UserResponse(Guid Id, string Name, string Email);
