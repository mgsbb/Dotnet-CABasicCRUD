using CABasicCRUD.Application.Common;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.SearchPosts;

public sealed record SearchPostsQuery(
    string? SearchTerm,
    int Page,
    int PageSize,
    PostOrderBy OrderBy,
    SortDirection SortDirection,
    UserId? UserId
) : IQuery<IReadOnlyList<PostWithAuthorResult>>;
