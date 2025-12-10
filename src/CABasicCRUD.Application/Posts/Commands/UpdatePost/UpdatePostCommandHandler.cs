using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
        }

        post.Update(title: request.UpdatePostDTO.Title, content: request.UpdatePostDTO.Content);

        await _postRepository.UpdateAsync(entity: post);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}
