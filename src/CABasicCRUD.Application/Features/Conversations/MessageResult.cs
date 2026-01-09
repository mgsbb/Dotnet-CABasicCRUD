using CABasicCRUD.Domain.Messages;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Conversations;

public sealed record MessageResult(
    MessageId Id,
    string Content,
    UserId SenderUserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
