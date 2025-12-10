using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Mapping;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Queries.GetPostById;

public class GetPostByIdQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetPostByIdQuery, PostDTO?>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<PostDTO?> Handle(
        GetPostByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var post = await _postRepository.GetByIdAsync(id: request.PostId);

        if (post is null)
        {
            throw new KeyNotFoundException($"Post with Id: {request.PostId.Value} not found");
        }

        var postDTO = post.ToPostDTO();

        return postDTO;
    }
}
