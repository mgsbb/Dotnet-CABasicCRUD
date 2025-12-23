using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Auth.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<AuthResult>;
