using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.MediaItems;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.DeletePost;

internal sealed class DeletePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IFileStorage fileStorage,
    IMediaRepository mediaRepository
) : ICommandHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;
    private readonly IMediaRepository _mediaRepository = mediaRepository;
    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        Post? post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result.Failure(Common.PostErrors.NotFound);
        }

        if (request.UserId != post.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        if (post.PostMediaItems.Any())
        {
            foreach (PostMedia postMedia in post.PostMediaItems)
            {
                Media? media = await _mediaRepository.GetByIdAsync(postMedia.MediaId);

                if (media is not null)
                {
                    await _fileStorage.DeleteAsync(media.StorageKey, cancellationToken);

                    await _mediaRepository.DeleteAsync(media);
                }
            }
        }

        await _postRepository.DeleteAsync(entity: post);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        string cacheKey = $"posts:{post.Id}";

        await _cacheService.RemoveAsync(cacheKey, cancellationToken);

        await _cacheService.RemoveAsync("posts:all", cancellationToken);

        return Result.Success();
    }
}
