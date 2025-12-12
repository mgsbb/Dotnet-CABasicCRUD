using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Mapping;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public class CreatePostCommandHandler(IPostRepository postRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePostCommand, Result<PostDTO>>
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<PostDTO>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<Post> postResult = request.CreatePostDTO.ToEntityResult();

        // Result<Post> postResult = Post.Create(
        //     title: request.CreatePostDTO.Title,
        //     content: request.CreatePostDTO.Content
        // );

        if (postResult.IsFailure || postResult.Value is null)
        {
            return Result<PostDTO>.Failure(postResult.Error);
        }

        Post post = postResult.Value;

        Post postFromDB = await _postRepository.AddAsync(entity: post);

        PostDTO postDTO = postFromDB.ToPostDTO();

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        return postDTO;
    }
}
