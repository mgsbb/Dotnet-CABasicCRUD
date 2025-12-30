using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Comments.UpdateComment;

internal sealed class UpdateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : ICommandHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<CommentResult>.Failure(AuthErrors.Unauthenticated);
        }

        var comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound);
        }

        if (comment.UserId != _currentUser.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        Result<Comment> result = comment.Update(request.Body);

        if (result.IsFailure || result.Value is null)
            return Result.Failure(result.Error);

        await _commentRepository.UpdateAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success();
    }
}
