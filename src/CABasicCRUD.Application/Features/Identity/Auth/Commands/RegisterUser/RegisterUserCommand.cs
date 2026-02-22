using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;

namespace CABasicCRUD.Application.Features.Identity.Auth.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string Name,
    string Username,
    string Email,
    string Password
) : ICommand<AuthResult>;
