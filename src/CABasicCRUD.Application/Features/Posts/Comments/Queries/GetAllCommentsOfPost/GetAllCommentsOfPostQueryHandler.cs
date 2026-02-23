using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Comments;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetAllCommentsOfPost;

internal sealed class GetAllCommentsOfPostQueryHandler(
    IPostReadService postReadService,
    ICommentReadService commentReadService
) : IQueryHander<GetAllCommentsOfPostQuery, IReadOnlyList<CommentResult>>
{
    private readonly IPostReadService _postReadService = postReadService;
    private readonly ICommentReadService _commentReadService = commentReadService;

    public async Task<Result<IReadOnlyList<CommentResult>>> Handle(
        GetAllCommentsOfPostQuery request,
        CancellationToken cancellationToken
    )
    {
        Post? post = await _postReadService.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<IReadOnlyList<CommentResult>>.Failure(Posts.Common.PostErrors.NotFound);
        }

        IReadOnlyList<Comment> comments = await _commentReadService.GetAllCommentsOfPost(
            request.PostId
        );

        IReadOnlyList<CommentResult> commentResults = comments.ToListCommentResult();

        return Result<IReadOnlyList<CommentResult>>.Success(commentResults);
    }
}
