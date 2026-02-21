namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public class PostDetailsViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public string Content { get; init; } = default!;
    public Guid UserId { get; init; }
    public string AuthorName { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public IReadOnlyList<CommentViewModel> Comments { get; init; } = [];

    public CommentCreateViewModel NewComment { get; set; } = new();
}
