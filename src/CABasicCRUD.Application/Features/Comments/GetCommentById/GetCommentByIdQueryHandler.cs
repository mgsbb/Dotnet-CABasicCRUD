using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Comments.GetCommentById;

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
            return Result<CommentResult>.Failure(CommentErrors.NotFound);
        }

        CommentResult commentResult = comment.ToCommentResult();

        return commentResult;
    }
}
