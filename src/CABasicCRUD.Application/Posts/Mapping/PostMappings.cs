using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Posts.Mapping;

public static class PostMappings
{
    public static Post ToEntity(this CreatePostDTO createPostDTO)
    {
        return Post.Create(title: createPostDTO.Title, content: createPostDTO.Content);
    }

    public static PostDTO ToPostDTO(this Post post)
    {
        return new PostDTO(PostId: post.Id, Title: post.Title, Content: post.Content);
    }

    public static IReadOnlyList<PostDTO> ToListPostDTO(this IReadOnlyList<Post> posts)
    {
        if (posts == null)
            return new List<PostDTO>();

        return posts.Select(post => post.ToPostDTO()).ToList();
    }
}
