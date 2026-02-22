using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(UserId UserId) : IQuery<UserResult>;
