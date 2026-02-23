using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Presentation.WebMvc.Models.Conversations;

public sealed class ConversationDetailsViewModel
{
    public Guid Id { get; init; }
    public ConversationType ConversationType { get; init; }
    public IReadOnlyList<ConversationParticipantViewModel> Participants { get; init; } = [];
    public IReadOnlyList<MessageViewModel> Messages { get; init; } = [];
    public DateTime CreatedAt;
    public DateTime? UpdatedAt;
    public MessageCreateViewModel NewMessage { get; init; } = new();
}
