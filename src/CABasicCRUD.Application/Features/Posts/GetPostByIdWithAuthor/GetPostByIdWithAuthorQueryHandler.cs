using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Posts.GetPostByIdWithAuthor;

internal sealed class GetPostByIdWithAuthorQueryHandler(IPostReadService postReadService)
    : IQueryHander<GetPostByIdWithAuthorQuery, PostWithAuthorResult>
{
    private readonly IPostReadService _postReadService = postReadService;

    public async Task<Result<PostWithAuthorResult>> Handle(
        GetPostByIdWithAuthorQuery request,
        CancellationToken cancellationToken
    )
    {
        var post = await _postReadService.GetPostByIdWithAuthor(request.PostId, cancellationToken);

        if (post is null)
        {
            return Result<PostWithAuthorResult>.Failure(PostErrors.NotFound);
        }

        return post;
    }
}
