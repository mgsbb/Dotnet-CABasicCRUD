using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.CreateComment;

internal sealed class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IPostReadService postReadService,
    IUserReadService userReadService,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCommentCommand, CommentResult>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IPostReadService _postReadService = postReadService;
    private readonly IUserReadService _userReadService = userReadService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CommentResult>> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userReadService.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<CommentResult>.Failure(Identity.Users.Common.UserErrors.NotFound);
        }

        Post? post = await _postReadService.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<CommentResult>.Failure(Posts.Common.PostErrors.NotFound);
        }

        Result<Comment> result = Comment.Create(request.Body, request.PostId, request.UserId);

        if (result.IsFailure)
        {
            return Result<CommentResult>.Failure(result.Error);
        }

        await _commentRepository.AddAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.ToCommentResult();
    }
}
