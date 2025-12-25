using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Presentation.WebAPI.Features.Posts.Contracts;

namespace CABasicCRUD.Presentation.WebAPI.Features.Posts;

internal static class PostMappings
{
    internal static PostResponse ToPostResponse(this PostResult postResult)
    {
        return new(postResult.Id, postResult.Title, postResult.Content);
    }

    internal static IReadOnlyList<PostResponse> ToListPostResult(
        this IReadOnlyList<PostResult> posts
    )
    {
        if (posts == null)
            return new List<PostResponse>();

        return posts.Select(post => post.ToPostResponse()).ToList();
    }
}
