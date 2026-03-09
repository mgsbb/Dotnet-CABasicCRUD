using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Identity.Users;
using SortDirection = CABasicCRUD.Application.Features.Posts.Posts.Common.SortDirection;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.SearchPosts;

public sealed record SearchPostsQuery(
    string? SearchTerm,
    int Page,
    int PageSize,
    PostOrderBy OrderBy,
    SortDirection SortDirection,
    UserId? UserId
) : IQuery<IReadOnlyList<PostWithAuthorResult>>;
