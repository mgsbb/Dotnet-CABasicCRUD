using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Conversations.Conversations.Commands.StartPrivateConversation;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Application.Features.Conversations.Conversations.Queries.GetConversationById;
using CABasicCRUD.Application.Features.Conversations.Messages.Commands.SendMessage;
using CABasicCRUD.Application.Features.Conversations.Messages.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Presentation.WebMvc.Models.Conversations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

[Authorize]
[Route("conversations")]
public sealed class ConversationsController(ICurrentUser currentUser, IMediator mediator)
    : Controller
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        GetConversationByIdQuery query = new((ConversationId)id);

        Result<ConversationResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }

        IReadOnlyList<MessageViewModel> messageViewModels = result
            .Value.Messages.Select(message => new MessageViewModel
            {
                Id = message.Id,
                Content = message.Content,
                SenderUserId = message.SenderUserId,
                CreatedAt = message.CreatedAt,
                UpdatedAt = message.UpdatedAt,
            })
            .ToList();

        ConversationDetailsViewModel model = new()
        {
            Id = result.Value.Id,
            CreatedAt = result.Value.CreatedAt,
            UpdatedAt = result.Value.UpdatedAt,
            Messages = messageViewModels,
            ParticipantsId = result.Value.ParticipantsId,
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> StartPrivateConversation(Guid targetUserId)
    {
        StartPrivateConversationCommand command = new(
            (UserId)_currentUser.UserId,
            (UserId)targetUserId
        );

        Result<ConversationResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }

        // result.Value contains messages, participant ids, etc, but only the id is being used
        // can instead just return ConversationId instead of a ConversationResult

        return RedirectToAction(nameof(Details), new { id = result.Value.Id.Value });
    }

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> SendMessage(
        [Bind(Prefix = "NewMessage")] MessageCreateViewModel model,
        Guid id
    )
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Details), new { id });
        }

        SendMessageCommand command = new((ConversationId)id, model.Content);

        Result<MessageResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Details), new { id });
    }
}
