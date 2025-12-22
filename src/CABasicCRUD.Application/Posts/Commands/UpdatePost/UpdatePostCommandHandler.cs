using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using PostErrors = CABasicCRUD.Application.Posts.Errors.PostErrors;

namespace CABasicCRUD.Application.Posts.Commands.UpdatePost;

internal sealed class UpdatePostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdatePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            // throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
            return Result.Failure(PostErrors.NotFound);
        }

        Result<Post> result = post.Update(title: request.Title, content: request.Content);

        if (result.IsFailure || result.Value == null)
            return Result.Failure(result.Error);

        await _postRepository.UpdateAsync(entity: result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return Result.Success();
    }
}
