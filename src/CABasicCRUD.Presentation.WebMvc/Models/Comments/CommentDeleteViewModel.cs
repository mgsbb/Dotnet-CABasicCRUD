namespace CABasicCRUD.Presentation.WebMvc.Models.Comments;

public class CommentDeleteViewModel
{
    public Guid Id { get; init; }
    public string Body { get; init; } = default!;
    public Guid PostId { get; init; }
}
