using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Messages.Common;

public sealed record MessageResult(
    MessageId Id,
    string Content,
    UserId SenderUserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
