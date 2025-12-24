using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts.GetAllposts;

internal sealed class GetAllPostsQueryHandler(IPostRepository postRepository)
    : IQueryHander<GetAllPostsQuery, IReadOnlyList<PostResult>>
{
    private readonly IPostRepository _postRepository = postRepository;

    public async Task<Result<IReadOnlyList<PostResult>>> Handle(
        GetAllPostsQuery request,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Post> posts = await _postRepository.GetAllAsync();

        IReadOnlyList<PostResult> postsList = posts.ToListPostResult();

        return Result<IReadOnlyList<PostResult>>.Success(postsList);
    }
}
