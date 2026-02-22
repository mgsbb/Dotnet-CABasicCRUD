using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(UserId UserId) : ICommand;
