using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.GetPostById;

internal sealed class GetPostByIdQueryHandler(
    IPostReadService postReadService,
    ICacheService cacheService
) : IQueryHander<GetPostByIdQuery, PostResult>
{
    private readonly IPostReadService _postReadService = postReadService;
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

        var postResult = await _postReadService.GetByIdWithMediaAsync(id: request.PostId);

        if (postResult is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result<PostResult>.Failure(Common.PostErrors.NotFound);
        }

        await _cacheService.SetAsync<PostResult>(cacheKey, postResult, cancellationToken);

        return postResult;
    }
}
