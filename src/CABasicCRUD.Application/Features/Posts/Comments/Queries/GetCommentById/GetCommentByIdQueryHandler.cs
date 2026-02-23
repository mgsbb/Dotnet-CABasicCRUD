using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetCommentById;

internal sealed class GetCommentByIdQueryHandler(ICommentReadService commentReadService)
    : IQueryHander<GetCommentByIdQuery, CommentResult>
{
    private readonly ICommentReadService _commentReadService = commentReadService;

    public async Task<Result<CommentResult>> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        Comment? comment = await _commentReadService.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result<CommentResult>.Failure(Common.CommentErrors.NotFound);
        }

        CommentResult commentResult = comment.ToCommentResult();

        return commentResult;
    }
}
