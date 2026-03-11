namespace CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;

public sealed record UpdateUserProfileRequest(string? FullName, string? Bio);
