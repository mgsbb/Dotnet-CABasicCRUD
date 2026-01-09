using CABasicCRUD.Application.Features.Conversations;
using CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Conversations;

internal static class ConversationMappings
{
    internal static ConversationResponse ToConversationResponse(
        this ConversationResult conversationResult
    )
    {
        return new(
            conversationResult.Id,
            conversationResult.ParticipantsId,
            conversationResult.Messages.ToListToMessageResponse(),
            conversationResult.CreatedAt,
            conversationResult.UpdatedAt
        );
    }

    internal static IReadOnlyList<ConversationResponse> ToListConversationResponse(
        this IReadOnlyList<ConversationResult> conversationResults
    )
    {
        if (conversationResults == null)
            return new List<ConversationResponse>();

        return conversationResults
            .Select(conversation => conversation.ToConversationResponse())
            .ToList();
    }

    internal static ConversationResponseWithoutMessages ToConversationResponseWithoutMessages(
        this ConversationResultWithoutMessages conversationResult
    )
    {
        return new(
            conversationResult.Id,
            conversationResult.ParticipantsId,
            conversationResult.CreatedAt,
            conversationResult.UpdatedAt
        );
    }

    internal static IReadOnlyList<ConversationResponseWithoutMessages> ToListConversationResponseWithoutMessages(
        this IReadOnlyList<ConversationResultWithoutMessages> conversationResults
    )
    {
        if (conversationResults == null)
            return new List<ConversationResponseWithoutMessages>();

        return conversationResults
            .Select(conversation => conversation.ToConversationResponseWithoutMessages())
            .ToList();
    }
}
