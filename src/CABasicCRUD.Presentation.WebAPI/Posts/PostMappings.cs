using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Presentation.WebAPI.Posts.Contracts;

namespace CABasicCRUD.Presentation.WebAPI.Posts;

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
