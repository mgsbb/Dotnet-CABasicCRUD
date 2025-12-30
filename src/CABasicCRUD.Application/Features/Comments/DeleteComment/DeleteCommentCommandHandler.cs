using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments.DeleteComment;

internal sealed class DeleteCommentCommandHandler(
    ICommentRepository commentRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<CommentResult>.Failure(AuthErrors.Unauthenticated);
        }

        User? user = await _userRepository.GetByIdAsync((UserId)_currentUser.UserId);

        if (user is null)
        {
            return Result<CommentResult>.Failure(Users.UserErrors.NotFound);
        }

        Comment? comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound);
        }

        if (_currentUser.UserId != comment.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        await _commentRepository.DeleteAsync(comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
