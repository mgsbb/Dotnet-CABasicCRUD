using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments.CreateComment;

internal sealed class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : ICommandHandler<CreateCommentCommand, CommentResult>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<CommentResult>> Handle(
        CreateCommentCommand request,
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

        Post? post = await _postRepository.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<CommentResult>.Failure(Posts.PostErrors.NotFound);
        }

        Result<Comment> result = Comment.Create(
            request.Body,
            request.PostId,
            (UserId)_currentUser.UserId
        );

        if (result.IsFailure || result.Value is null)
        {
            return Result<CommentResult>.Failure(result.Error);
        }

        await _commentRepository.AddAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.ToCommentResult();
    }
}
