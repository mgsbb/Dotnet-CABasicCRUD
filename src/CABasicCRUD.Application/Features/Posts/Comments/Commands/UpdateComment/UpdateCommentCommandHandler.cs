using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.UpdateComment;

internal sealed class UpdateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result.Failure(Common.CommentErrors.NotFound);
        }

        if (comment.UserId != request.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        Result<Comment> result = comment.Update(request.Body);

        if (result.IsFailure || result.Value is null)
            return Result.Failure(result.Error);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success();
    }
}
