using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Features.Auth.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<AuthResult>;
