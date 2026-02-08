using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.UpdatePost;

internal sealed class UpdatePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService
) : ICommandHandler<UpdatePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result.Failure(PostErrors.NotFound);
        }

        if (post.UserId != request.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        Result<Post> result = post.Update(title: request.Title, content: request.Content);

        if (result.IsFailure || result.Value == null)
            return Result.Failure(result.Error);

        await _postRepository.UpdateAsync(entity: result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        string cacheKey = $"posts:{post.Id}";

        await _cacheService.RemoveAsync(cacheKey, cancellationToken);

        await _cacheService.RemoveAsync("posts:all", cancellationToken);

        return Result.Success();
    }
}
