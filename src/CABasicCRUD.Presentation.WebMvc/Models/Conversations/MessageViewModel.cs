namespace CABasicCRUD.Presentation.WebMvc.Models.Conversations;

public sealed class MessageViewModel
{
    public Guid Id { get; init; }
    public string Content { get; init; } = default!;
    public Guid SenderUserId { get; init; }
    public string SenderUsername { get; init; } = default!;
    public string SenderFullName { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
