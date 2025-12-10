using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Posts.DTOs;

public record PostDTO(PostId PostId, string Title, string Content);
