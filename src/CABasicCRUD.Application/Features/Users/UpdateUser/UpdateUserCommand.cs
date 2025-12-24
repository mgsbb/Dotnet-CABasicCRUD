using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(UserId UserId, string Name, string Email) : ICommand;
