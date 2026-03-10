using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Conversations.Conversations.Commands.CreateConversation;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Application.Features.Conversations.Conversations.Queries;
using CABasicCRUD.Application.Features.Conversations.Conversations.Queries.GetConversationsOfUser;
using CABasicCRUD.Application.Features.Conversations.Messages.Commands.SendMessage;
using CABasicCRUD.Application.Features.Conversations.Messages.Common;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ConversationErrorsApp = CABasicCRUD.Application.Features.Conversations.Conversations.Common.ConversationErrors;
using ConversationErrorsDomain = CABasicCRUD.Domain.Conversations.Conversations.ConversationErrors;

namespace CABasicCRUD.Presentation.WebApi.Features.Conversations;

// ========================================================================================================================
// ========================================================================================================================

[EnableRateLimiting(RateLimitPolicies.Authenticated)]
[ApiController]
[Route("/api/v1/[controller]")]
public class ConversationsController(IMediator mediator, ICurrentUser currentUser) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUser _currentUser = currentUser;

    // ========================================================================================================================

    [Authorize]
    [HttpPost]
    [ProducesResponseType(
        type: typeof(ConversationResponse),
        statusCode: StatusCodes.Status201Created
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConversationResponse>> CreateConversation(
        [FromBody] CreateConversationRequest request
    )
    {
        IReadOnlyList<UserId> participantUserIds = request
            .ParticipantUserIds.Select(userGuid => (UserId)userGuid)
            .ToList();

        Enum.TryParse<ConversationType>(request.ConversationType, true, out var conversationType);

        CreateConversationCommand command = new(
            (UserId)_currentUser.UserId,
            participantUserIds,
            conversationType,
            request.GroupTitle
        );

        Result<ConversationResult> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        ConversationResponse conversationResponse = result.Value.ToConversationResponse();

        return CreatedAtAction(
            actionName: nameof(GetConversationById),
            routeValues: new { id = conversationResponse.Id },
            value: conversationResponse
        );
    }

    // ========================================================================================================================

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(
        type: typeof(ConversationDetailsResponse),
        statusCode: StatusCodes.Status200OK
    )]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversationDetailsResponse?>> GetConversationById(Guid id)
    {
        GetConversationByIdWithDetailsQuery query = new((ConversationId)id);

        Result<ConversationDetailsResult> result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return result.Value.ToConversationDetailsResponse();
    }

    // ========================================================================================================================

    [Authorize]
    [HttpGet]
    [ProducesResponseType(
        type: typeof(IReadOnlyList<ConversationResponseWithoutMessages>),
        statusCode: StatusCodes.Status200OK
    )]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<
        ActionResult<IReadOnlyList<ConversationResponseWithoutMessages>>
    > GetConversationsOfUser()
    {
        GetConversationsOfUserQuery query = new();

        Result<IReadOnlyList<ConversationResultWithoutMessages>> result = await _mediator.Send(
            query
        );

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return Ok(result.Value.ToListConversationResponseWithoutMessages());
    }

    // ========================================================================================================================

    [Authorize]
    [HttpPost("{id}/messages")]
    [ProducesResponseType(
        type: typeof(ConversationResponse),
        statusCode: StatusCodes.Status201Created
    )]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageResponse>> SendMessage(
        [FromBody] SendMessageRequest request,
        Guid id
    )
    {
        SendMessageCommand command = new((ConversationId)id, request.Content);

        Result<MessageResult> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return Ok(result.Value.ToMessageResponse());
    }

    // ========================================================================================================================

    private ObjectResult HandleResultFailure(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }
        if (result is IValidationResult validationResult)
        {
            IDictionary<string, object?> extensions = new Dictionary<string, object?>
            {
                ["errors"] = validationResult
                    .Errors.Select(e => new { code = e.Code, message = e.Message })
                    .ToList(),
            };
            return HandleProblem(
                StatusCodes.Status400BadRequest,
                detail: result.Error?.Message,
                extensions: extensions
            );
        }
        if (result.Error == AuthErrors.Unauthenticated)
        {
            return HandleProblem(StatusCodes.Status401Unauthorized, detail: result.Error.Message);
        }
        if (result.Error == AuthErrors.Forbidden)
        {
            return HandleProblem(StatusCodes.Status403Forbidden, detail: result.Error.Message);
        }
        if (result.Error == ConversationErrorsApp.NotFound)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }
        if (result.Error == Application.Features.Identity.Users.Common.UserErrors.NotFound)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }
        if (result.Error == ConversationErrorsApp.ConversationWithSelf)
        {
            return HandleProblem(StatusCodes.Status409Conflict, detail: result.Error.Message);
        }
        if (result.Error == ConversationErrorsDomain.NotAParticipant)
        {
            return HandleProblem(StatusCodes.Status403Forbidden, detail: result.Error.Message);
        }
        if (
            result.Error == ConversationErrorsApp.InvalidParticipantCount
            || result.Error == ConversationErrorsDomain.CreatorMustBeParticipant
        )
        {
            return HandleProblem(StatusCodes.Status400BadRequest, detail: result.Error.Message);
        }
        return HandleProblem(StatusCodes.Status500InternalServerError);
    }
}

// ========================================================================================================================
// ========================================================================================================================
