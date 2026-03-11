using CABasicCRUD.Application.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;

namespace CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;

public sealed record SearchUsersRequestQueryParams(
    string? SearchTerm,
    int Page = 1,
    int PageSize = 10,
    UserOrderBy UserOrderBy = UserOrderBy.CreatedAt,
    SortDirection SortDirection = SortDirection.Desc
);
