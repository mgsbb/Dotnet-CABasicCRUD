using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments.CreateComment;

internal sealed class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCommentCommand, CommentResult>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CommentResult>> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<CommentResult>.Failure(Users.UserErrors.NotFound);
        }

        Post? post = await _postRepository.GetByIdAsync(request.PostId);

        if (post is null)
        {
            return Result<CommentResult>.Failure(Posts.PostErrors.NotFound);
        }

        Result<Comment> result = Comment.Create(request.Body, request.PostId, request.UserId);

        if (result.IsFailure || result.Value is null)
        {
            return Result<CommentResult>.Failure(result.Error);
        }

        await _commentRepository.AddAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.ToCommentResult();
    }
}
