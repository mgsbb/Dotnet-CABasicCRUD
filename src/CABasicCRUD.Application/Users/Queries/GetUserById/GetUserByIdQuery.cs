using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Users.DTOs;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(UserId UserId) : IQuery<UserResult>;
