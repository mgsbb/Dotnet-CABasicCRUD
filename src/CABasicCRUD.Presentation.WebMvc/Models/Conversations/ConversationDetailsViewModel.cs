namespace CABasicCRUD.Presentation.WebMvc.Models.Conversations;

public sealed class ConversationDetailsViewModel
{
    public Guid Id { get; init; }
    public IReadOnlyList<Guid> ParticipantsId { get; init; } = [];
    public IReadOnlyList<MessageViewModel> Messages { get; init; } = [];
    public DateTime CreatedAt;
    public DateTime? UpdatedAt;
    public MessageCreateViewModel NewMessage { get; init; } = new();
}
