using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Queries.SearchUsers;

public sealed record SearchUsersQuery(
    string SearchTerm,
    int Page,
    int PageSize,
    UserOrderBy OrderBy,
    SortDirection SortDirection
) : IQuery<IReadOnlyList<UserResult>>;
