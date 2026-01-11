using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.GetAllposts;

internal sealed class GetAllPostsQueryHandler(
    IPostRepository postRepository,
    ICacheService cacheService
) : IQueryHander<GetAllPostsQuery, IReadOnlyList<PostResult>>
{
    private readonly IPostRepository _postRepository = postRepository;
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

        IReadOnlyList<Post> posts = await _postRepository.GetAllAsync();

        IReadOnlyList<PostResult> postsList = posts.ToListPostResult();

        await _cacheService.SetAsync(cacheKey, postsList, cancellationToken);

        return Result<IReadOnlyList<PostResult>>.Success(postsList);
    }
}
