using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Mapping;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using MediatR;

namespace CABasicCRUD.Application.Posts.Queries.GetAllposts;

public class GetAllPostsQueryHandler(IPostRepository postRepository)
    : IRequestHandler<GetAllPostsQuery, Result<IReadOnlyList<PostDTO>>>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<Result<IReadOnlyList<PostDTO>>> Handle(
        GetAllPostsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Post> posts = await _postRepository.GetAllAsync();

        IReadOnlyList<PostDTO> postDTOs = posts.ToListPostDTO();

        return Result<IReadOnlyList<PostDTO>>.Success(postDTOs);
    }
}
