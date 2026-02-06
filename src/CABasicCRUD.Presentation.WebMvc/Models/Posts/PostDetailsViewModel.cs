namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public class PostDetailsViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public string Content { get; init; } = default!;
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
