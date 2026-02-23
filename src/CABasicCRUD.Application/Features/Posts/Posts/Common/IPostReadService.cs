using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Common;

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

    Task<PostWithAuthorResult?> GetPostByIdWithAuthor(
        PostId postId,
        CancellationToken cancellationToken
    );

    Task<IReadOnlyList<Post>> GetAllAsync();

    Task<Post?> GetByIdAsync(PostId id);
}
