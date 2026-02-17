using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Domain.Posts;

public interface IPostRepository : IRepository<Post, PostId>
{
    Task<IReadOnlyList<Post>> SearchPostsAsync(
        string searchTerm,
        int page,
        int pageSize,
        PostOrderBy orderBy,
        SortDirection sortDirection,
        UserId? userId,
        CancellationToken cancellationToken
    );
}
