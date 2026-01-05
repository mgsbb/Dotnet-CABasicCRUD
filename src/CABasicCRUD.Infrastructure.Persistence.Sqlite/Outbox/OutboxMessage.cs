namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = default!;
    public string Content { get; init; } = default!;
    public DateTime OccurredOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}
