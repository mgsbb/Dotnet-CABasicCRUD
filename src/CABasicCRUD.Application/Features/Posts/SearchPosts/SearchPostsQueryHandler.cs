using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.SearchPosts;

internal sealed class SearchPostsQueryHandler(IPostRepository postRepository)
    : IQueryHander<SearchPostsQuery, IReadOnlyList<PostResult>>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<Result<IReadOnlyList<PostResult>>> Handle(
        SearchPostsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Post> posts = await _postRepository.SearchPostsAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            request.OrderBy,
            request.SortDirection,
            request.UserId,
            cancellationToken
        );

        IReadOnlyList<PostResult> postResults = posts.ToListPostResult();

        return Result<IReadOnlyList<PostResult>>.Success(postResults);
    }
}
