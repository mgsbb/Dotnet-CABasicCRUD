using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.MediaItems;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.CreatePost;

internal sealed class CreatePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService,
    IFileStorage fileStorage,
    IMediaRepository mediaRepository
) : ICommandHandler<CreatePostCommand, PostResult>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    private readonly IFileStorage _fileStorage = fileStorage;
    private readonly IMediaRepository _mediaRepository = mediaRepository;

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

        if (result.IsFailure)
        {
            // Error may be any of the domain errors - TitleEmpty, ContentEmpty, etc. How to handle this in the controller?
            return Result<PostResult>.Failure(result.Error);
        }

        Post post = result.Value;

        Post postFromDB = await _postRepository.AddAsync(entity: post);

        List<string> mediaUrls = [];

        if (request.CreatePostMedia is not null)
            foreach (var file in request.CreatePostMedia)
            {
                UploadResult uploadResult = await _fileStorage.UploadAsync(
                    file.Stream,
                    file.FileName,
                    file.MediaType == MediaType.Image ? "image" : "video",
                    cancellationToken
                );

                Media media = Media.Create(
                    StorageProvider.Cloudinary,
                    uploadResult.Key,
                    uploadResult.Url,
                    file.MediaType,
                    file.FileName,
                    file.Stream.Length,
                    file.ContentType
                );

                await _mediaRepository.AddAsync(media);

                post.AddMedia(media.Id);

                mediaUrls.Add(uploadResult.Url);
            }

        PostResult postResult = new(
            postFromDB.Id,
            postFromDB.Title,
            postFromDB.Content,
            postFromDB.UserId,
            postFromDB.CreatedAt,
            postFromDB.UpdatedAt,
            mediaUrls.AsReadOnly()
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        await _cacheService.RemoveAsync("posts:all", cancellationToken);

        return postResult;
    }
}
