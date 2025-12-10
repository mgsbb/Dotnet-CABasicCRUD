using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Mapping;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public class CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePostCommand, PostDTO>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PostDTO> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken
    )
    {
        Post post = request.CreatePostDTO.ToEntity();

        Post postFromDB = await _postRepository.AddAsync(entity: post);

        PostDTO postDTO = postFromDB.ToPostDTO();

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return postDTO;
    }
}
