using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Posts.SearchPosts;

internal sealed class SearchPostsQueryHandler(IPostReadService postReadService)
    : IQueryHander<SearchPostsQuery, IReadOnlyList<PostWithAuthorResult>>
{
    private readonly IPostReadService _postReadService = postReadService;

    public async Task<Result<IReadOnlyList<PostWithAuthorResult>>> Handle(
        SearchPostsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<PostWithAuthorResult> posts = await _postReadService.SearchPostsAsync(
            request.SearchTerm,
            request.Page,
            request.PageSize,
            request.OrderBy,
            request.SortDirection,
            request.UserId,
            cancellationToken
        );

        return Result<IReadOnlyList<PostWithAuthorResult>>.Success(posts);
    }
}
