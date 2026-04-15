using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.GetAllPosts;

internal sealed class GetAllPostsQueryHandler(
    IPostReadService postReadService,
    ICacheService cacheService
) : IQueryHander<GetAllPostsQuery, IReadOnlyList<PostResult>>
{
    private readonly IPostReadService _postReadService = postReadService;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result<IReadOnlyList<PostResult>>> Handle(
        GetAllPostsQuery request,
        CancellationToken cancellationToken
    )
    {
        string cacheKey = $"posts:all";

        IReadOnlyList<PostResult>? cached = await _cacheService.GetAsync<IReadOnlyList<PostResult>>(
            cacheKey,
            cancellationToken
        );

        if (cached is not null)
        {
            return Result<IReadOnlyList<PostResult>>.Success(cached);
        }

        IReadOnlyList<PostResult> posts = await _postReadService.GetAllWithMediaAsync();

        await _cacheService.SetAsync(cacheKey, posts, cancellationToken);

        return Result<IReadOnlyList<PostResult>>.Success(posts);
    }
}
