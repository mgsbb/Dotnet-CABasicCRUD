using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Posts;

internal static class PostMappings
{
    internal static PostResponse ToPostResponse(this PostResult postResult)
    {
        return new(
            postResult.Id,
            postResult.Title,
            postResult.Content,
            postResult.UserId,
            postResult.CreatedAt,
            postResult.UpdatedAt,
            postResult.MediaUrls
        );
    }

    internal static IReadOnlyList<PostResponse> ToListPostResponse(
        this IReadOnlyList<PostResult> posts
    )
    {
        if (posts == null)
            return new List<PostResponse>();

        return posts.Select(post => post.ToPostResponse()).ToList();
    }

    internal static PostWithAuthorResponse ToPostWithAuthorResponse(
        this PostWithAuthorResult postWithAuthorResult
    )
    {
        return new(
            postWithAuthorResult.Id,
            postWithAuthorResult.Title,
            postWithAuthorResult.Content,
            postWithAuthorResult.UserId,
            postWithAuthorResult.AuthorName,
            postWithAuthorResult.AuthorProfileImageUrl,
            postWithAuthorResult.CreatedAt,
            postWithAuthorResult.UpdatedAt,
            postWithAuthorResult.MediaUrls
        );
    }

    internal static IReadOnlyList<PostWithAuthorResponse> ToListPostWithAuthorResponse(
        this IReadOnlyList<PostWithAuthorResult> posts
    )
    {
        if (posts == null)
            return new List<PostWithAuthorResponse>();

        return posts.Select(post => post.ToPostWithAuthorResponse()).ToList();
    }
}
