using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;

namespace CABasicCRUD.Application.Features.Identity.Auth.Commands.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<AuthResult>;
