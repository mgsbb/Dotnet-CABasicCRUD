using CABasicCRUD.Application.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Common;

namespace CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;

public sealed record SearchPostsRequestQueryParams(
    string? SearchTerm,
    int Page = 1,
    int PageSize = 10,
    PostOrderBy PostOrderBy = PostOrderBy.CreatedAt,
    SortDirection SortDirection = SortDirection.Desc,
    Guid? UserId = null
);
