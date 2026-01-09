using CABasicCRUD.Application.Features.Conversations;
using CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Conversations;

internal static class MessageMappings
{
    internal static MessageResponse ToMessageResponse(this MessageResult messageResult)
    {
        return new(
            messageResult.Id,
            messageResult.SenderUserId,
            messageResult.Content,
            messageResult.CreatedAt,
            messageResult.UpdatedAt
        );
    }

    internal static IReadOnlyList<MessageResponse> ToListToMessageResponse(
        this IReadOnlyList<MessageResult> messageResults
    )
    {
        if (messageResults == null)
            return new List<MessageResponse>();

        return messageResults.Select(messageResult => messageResult.ToMessageResponse()).ToList();
    }
}
