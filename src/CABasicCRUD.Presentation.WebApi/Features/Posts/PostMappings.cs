using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Posts;

internal static class PostMappings
{
    internal static PostResponse ToPostResponse(this PostResult postResult)
    {
        return new(postResult.Id, postResult.Title, postResult.Content, postResult.UserId);
    }

    internal static IReadOnlyList<PostResponse> ToListPostResponse(
        this IReadOnlyList<PostResult> posts
    )
    {
        if (posts == null)
            return new List<PostResponse>();

        return posts.Select(post => post.ToPostResponse()).ToList();
    }
}
