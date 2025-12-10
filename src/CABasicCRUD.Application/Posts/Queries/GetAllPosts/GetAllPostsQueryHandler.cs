using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Mapping;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Queries.GetAllposts;

public class GetAllPostsQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetAllPostsQuery, IReadOnlyList<PostDTO>>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<IReadOnlyList<PostDTO>> Handle(
        GetAllPostsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Post> posts = await _postRepository.GetAllAsync();

        IReadOnlyList<PostDTO> postDTOs = posts.ToListPostDTO();

        return postDTOs;
    }
}
