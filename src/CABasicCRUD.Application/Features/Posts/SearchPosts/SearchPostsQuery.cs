using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts.SearchPosts;

public sealed record SearchPostsQuery(
    string SearchTerm,
    int Page,
    int PageSize,
    PostOrderBy OrderBy,
    SortDirection SortDirection,
    UserId? UserId
) : IQuery<IReadOnlyList<PostWithAuthorResult>>;
