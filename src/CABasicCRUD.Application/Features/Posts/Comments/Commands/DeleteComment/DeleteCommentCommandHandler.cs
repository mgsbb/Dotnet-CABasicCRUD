using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.DeleteComment;

internal sealed class DeleteCommentCommandHandler(
    ICommentRepository commentRepository,
    IUserReadService userReadService,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUserReadService _userReadService = userReadService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userReadService.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<CommentResult>.Failure(Identity.Users.Common.UserErrors.NotFound);
        }

        Comment? comment = await _commentRepository.GetByIdAsync(request.Id);

        if (comment is null)
        {
            return Result.Failure(Common.CommentErrors.NotFound);
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
