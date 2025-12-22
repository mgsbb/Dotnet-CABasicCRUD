using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Auth.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Name, string Email, string Password)
    : ICommand<UserResult>;
