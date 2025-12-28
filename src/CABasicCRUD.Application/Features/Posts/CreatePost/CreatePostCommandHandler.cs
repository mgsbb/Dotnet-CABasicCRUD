using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts.CreatePost;

internal sealed class CreatePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : ICommandHandler<CreatePostCommand, PostResult>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<PostResult>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<PostResult>.Failure(AuthErrors.Unauthenticated);
        }

        Result<Post> result = Post.Create(
            title: request.Title,
            content: request.Content,
            userId: (UserId)_currentUser.UserId
        );

        // Result<Post> postResult = Post.Create(
        //     title: request.CreatePostDTO.Title,
        //     content: request.CreatePostDTO.Content
        // );

        if (result.IsFailure || result.Value is null)
        {
            // Error may be any of the domain errors - TitleEmpty, ContentEmpty, etc. How to handle this in the controller?
            return Result<PostResult>.Failure(result.Error);
        }

        Post post = result.Value;

        Post postFromDB = await _postRepository.AddAsync(entity: post);

        PostResult postResult = postFromDB.ToPostResult();

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return postResult;
    }
}
