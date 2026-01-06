using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Posts;

internal static class PostMappings
{
    internal static PostResult ToPostResult(this Post post)
    {
        return new PostResult(
            Id: post.Id,
            Title: post.Title,
            Content: post.Content,
            UserId: post.UserId,
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt
        );
    }

    internal static IReadOnlyList<PostResult> ToListPostResult(this IReadOnlyList<Post> posts)
    {
        if (posts == null)
            return new List<PostResult>();

        return posts.Select(post => post.ToPostResult()).ToList();
    }
}
