using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.DeletePost;

internal sealed class DeletePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService
) : ICommandHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        Post? post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result.Failure(PostErrors.NotFound);
        }

        if (request.UserId != post.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        await _postRepository.DeleteAsync(entity: post);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        string cacheKey = $"posts:{post.Id}";

        await _cacheService.RemoveAsync(cacheKey, cancellationToken);

        await _cacheService.RemoveAsync("posts:all", cancellationToken);

        return Result.Success();
    }
}
