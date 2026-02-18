namespace CABasicCRUD.Presentation.WebMvc.Models.Posts;

public class PostListItemViewModel
{
    public Guid Id { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string ContentPreview { get; init; } = default!;

    public Guid UserId { get; init; } = default!;

    public string Username { get; init; } = default!;
}
