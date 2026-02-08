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
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<CommentResult>.Failure(Users.UserErrors.NotFound);
        }

        Comment? comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound);
        }

        if (request.UserId != comment.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        await _commentRepository.DeleteAsync(comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
