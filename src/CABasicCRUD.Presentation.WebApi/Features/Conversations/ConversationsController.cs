using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Conversations;
using CABasicCRUD.Application.Features.Conversations.CreateConversation;
using CABasicCRUD.Application.Features.Conversations.GetConversationById;
using CABasicCRUD.Application.Features.Conversations.GetConversationsOfUser;
using CABasicCRUD.Application.Features.Conversations.SendMessage;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CABasicCRUD.Presentation.WebApi.Features.Conversations;

[EnableRateLimiting(RateLimitPolicies.Authenticated)]
[ApiController]
[Route("/api/v1/[controller]")]
public class ConversationsController(IMediator mediator) : ApiController
{
    private readonly IMediator _mediator = mediator;

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
        CreateConversationCommand command = new((UserId)request.UserId);

        Result<ConversationResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
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

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(ConversationResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConversationResponse?>> GetConversationById(Guid id)
    {
        GetConversationByIdQuery query = new((ConversationId)id);

        Result<ConversationResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
        }

        return result.Value.ToConversationResponse();
    }

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

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
        }

        return Ok(result.Value.ToListConversationResponseWithoutMessages());
    }

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

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
        }

        return Ok(result.Value.ToMessageResponse());
    }

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
        if (result.Error == Application.Features.Conversations.ConversationErrors.NotFound)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }
        if (result.Error == Application.Features.Users.UserErrors.NotFound)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }
        if (
            result.Error
            == Application.Features.Conversations.ConversationErrors.ConversationWithSelf
        )
        {
            return HandleProblem(StatusCodes.Status409Conflict, detail: result.Error.Message);
        }
        if (result.Error == Domain.Conversations.ConversationErrors.NotAParticipant)
        {
            return HandleProblem(StatusCodes.Status403Forbidden, detail: result.Error.Message);
        }
        return HandleProblem(StatusCodes.Status500InternalServerError);
    }
}
