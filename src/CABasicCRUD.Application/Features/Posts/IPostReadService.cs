using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts;

public interface IPostReadService
{
    Task<IReadOnlyList<PostWithAuthorResult>> SearchPostsAsync(
        string searchTerm,
        int page,
        int pageSize,
        PostOrderBy orderBy,
        SortDirection sortDirection,
        UserId? userId,
        CancellationToken cancellationToken
    );
}
