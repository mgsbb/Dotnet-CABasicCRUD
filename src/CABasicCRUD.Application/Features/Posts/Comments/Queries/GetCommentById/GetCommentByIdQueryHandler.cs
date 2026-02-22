using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetCommentById;

internal sealed class GetCommentByIdQueryHandler(ICommentRepository commentRepository)
    : IQueryHander<GetCommentByIdQuery, CommentResult>
{
    private readonly ICommentRepository _commentRepository = commentRepository;

    public async Task<Result<CommentResult>> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        Comment? comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result<CommentResult>.Failure(Common.CommentErrors.NotFound);
        }

        CommentResult commentResult = comment.ToCommentResult();

        return commentResult;
    }
}
