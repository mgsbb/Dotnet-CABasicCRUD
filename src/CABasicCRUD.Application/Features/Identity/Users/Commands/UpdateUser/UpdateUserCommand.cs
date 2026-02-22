using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(UserId UserId, string Name, string Email) : ICommand;
