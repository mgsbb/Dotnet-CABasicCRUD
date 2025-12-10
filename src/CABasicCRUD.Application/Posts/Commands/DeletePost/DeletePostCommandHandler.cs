using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.DeletePost;

public class DeletePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        Post? post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
        }

        await _postRepository.DeleteAsync(entity: post);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}
