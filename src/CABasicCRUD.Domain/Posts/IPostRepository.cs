using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts;

public interface IPostRepository : IRepository<Post, PostId>
{
    Task<IReadOnlyList<Post>> SearchPostsAsync(
        string searchTerm,
        int page,
        int pageSize,
        PostOrderBy orderBy,
        SortDirection sortDirection,
        CancellationToken cancellationToken
    );
}
