using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users.SearchUsers;

public sealed record SearchUsersQuery(
    string SearchTerm,
    int Page,
    int PageSize,
    UserOrderBy OrderBy,
    SortDirection SortDirection
) : IQuery<IReadOnlyList<UserResult>>;
