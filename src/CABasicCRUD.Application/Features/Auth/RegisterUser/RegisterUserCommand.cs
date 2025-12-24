using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Features.Auth.RegisterUser;

public sealed record RegisterUserCommand(string Name, string Email, string Password)
    : ICommand<AuthResult>;
