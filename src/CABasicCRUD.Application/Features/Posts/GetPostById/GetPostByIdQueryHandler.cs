using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.GetPostById;

internal sealed class GetPostByIdQueryHandler(
    IPostRepository postRepository,
    ICacheService cacheService
) : IQueryHander<GetPostByIdQuery, PostResult>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result<PostResult>> Handle(
        GetPostByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        string cacheKey = $"posts:{request.PostId}";

        var cached = await _cacheService.GetAsync<PostResult>(cacheKey, cancellationToken);

        if (cached is not null)
        {
            return cached;
        }

        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result<PostResult>.Failure(PostErrors.NotFound);
        }

        var postResult = post.ToPostResult();

        await _cacheService.SetAsync<PostResult>(cacheKey, postResult, cancellationToken);

        return postResult;
    }
}
