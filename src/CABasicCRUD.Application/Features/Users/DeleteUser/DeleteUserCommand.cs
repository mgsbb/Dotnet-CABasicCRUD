using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.DeleteUser;

public sealed record DeleteUserCommand(UserId UserId) : ICommand;
