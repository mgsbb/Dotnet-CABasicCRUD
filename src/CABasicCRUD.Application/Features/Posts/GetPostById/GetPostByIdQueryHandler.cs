using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.GetPostById;

internal sealed class GetPostByIdQueryHandler(IPostRepository postRepository)
    : IQueryHander<GetPostByIdQuery, PostResult>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<Result<PostResult>> Handle(
        GetPostByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result<PostResult>.Failure(PostErrors.NotFound);
        }

        var postResult = post.ToPostResult();

        return postResult;
    }
}
