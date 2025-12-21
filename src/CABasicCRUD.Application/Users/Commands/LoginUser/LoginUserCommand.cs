using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Users.DTOs;

namespace CABasicCRUD.Application.Users.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResult>;
