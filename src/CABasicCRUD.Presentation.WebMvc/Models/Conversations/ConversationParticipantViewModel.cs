namespace CABasicCRUD.Presentation.WebMvc.Models.Conversations;

public sealed class ConversationParticipantViewModel
{
    public Guid ParticipantUserId { get; init; }
    public string ParticipantFullName { get; init; } = default!;
    public string ParticipantUsername { get; init; } = default!;
}
