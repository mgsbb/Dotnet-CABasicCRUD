using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.CreatePost;

internal sealed class CreatePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService
) : ICommandHandler<CreatePostCommand, PostResult>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result<PostResult>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<Post> result = Post.Create(
            title: request.Title,
            content: request.Content,
            userId: request.UserId
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

        await _cacheService.RemoveAsync("posts:all", cancellationToken);

        return postResult;
    }
}
