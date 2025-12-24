using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(UserId UserId) : IQuery<UserResult>;
