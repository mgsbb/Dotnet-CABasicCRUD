using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetCommentsWithAuthorOfPost;

internal sealed class GetCommentsWithAuthorOfPostQueryHandler(
    IPostReadService postReadService,
    ICommentReadService commentReadService
) : IQueryHander<GetCommentsWithAuthorOfPostQuery, IReadOnlyList<CommentWithAuthorResult>>
{
    private readonly IPostReadService _postReadService = postReadService;
    private readonly ICommentReadService _commentReadService = commentReadService;

    public async Task<Result<IReadOnlyList<CommentWithAuthorResult>>> Handle(
        GetCommentsWithAuthorOfPostQuery request,
        CancellationToken cancellationToken
    )
    {
        Post? post = await _postReadService.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<IReadOnlyList<CommentWithAuthorResult>>.Failure(
                Posts.Common.PostErrors.NotFound
            );
        }

        IReadOnlyList<CommentWithAuthorResult> comments =
            await _commentReadService.GetCommentsWithAuthorOfPostAsync(
                request.PostId,
                cancellationToken
            );

        return Result<IReadOnlyList<CommentWithAuthorResult>>.Success(comments);
    }
}
