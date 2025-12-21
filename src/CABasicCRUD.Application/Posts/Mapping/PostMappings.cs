using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Posts.Mapping;

internal static class PostMappings
{
    internal static Result<Post> ToEntityResult(this CreatePostDTO createPostDTO)
    {
        Result<Post> resultPost = Post.Create(
            title: createPostDTO.Title,
            content: createPostDTO.Content
        );

        return resultPost;
    }

    internal static PostDTO ToPostDTO(this Post post)
    {
        return new PostDTO(Id: post.Id, Title: post.Title, Content: post.Content);
    }

    internal static IReadOnlyList<PostDTO> ToListPostDTO(this IReadOnlyList<Post> posts)
    {
        if (posts == null)
            return new List<PostDTO>();

        return posts.Select(post => post.ToPostDTO()).ToList();
    }
}
