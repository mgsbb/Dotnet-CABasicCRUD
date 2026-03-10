using CABasicCRUD.Application.Features.Conversations.Conversations.Queries;
using CABasicCRUD.Application.Features.Conversations.Messages.Common;
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

    internal static MessageDetailResponse ToMessageDetailResponse(this MessageDetail messageDetail)
    {
        return new(
            messageDetail.Id.Value,
            messageDetail.Content,
            messageDetail.SenderUserId,
            messageDetail.SenderUsername,
            messageDetail.SenderFullName,
            messageDetail.CreatedAt,
            messageDetail.UpdatedAt
        );
    }

    internal static IReadOnlyList<MessageDetailResponse> ToListMessageDetailResponse(
        this IReadOnlyList<MessageDetail> messageDetails
    )
    {
        return messageDetails.Select(m => m.ToMessageDetailResponse()).ToList();
    }
}
