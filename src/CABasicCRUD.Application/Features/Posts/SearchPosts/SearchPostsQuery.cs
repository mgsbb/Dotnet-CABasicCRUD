using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using SortDirection = CABasicCRUD.Domain.Posts.SortDirection;

namespace CABasicCRUD.Application.Features.Posts.SearchPosts;

public sealed record SearchPostsQuery(
    string SearchTerm,
    int Page,
    int PageSize,
    PostOrderBy OrderBy,
    SortDirection SortDirection,
    UserId? UserId
) : IQuery<IReadOnlyList<PostResult>>;
