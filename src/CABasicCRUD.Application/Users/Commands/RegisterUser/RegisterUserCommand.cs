using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Users.DTOs;

namespace CABasicCRUD.Application.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Name, string Email, string Password)
    : ICommand<UserResult>;
