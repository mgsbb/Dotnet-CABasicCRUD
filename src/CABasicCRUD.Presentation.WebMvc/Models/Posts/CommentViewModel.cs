namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public sealed class CommentViewModel
{
    public Guid Id { get; init; }
    public string Body { get; init; } = default!;
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}
