using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Conversations;
using CABasicCRUD.Application.Features.Conversations.StartPrivateConversation;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
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
        Console.WriteLine(id);
        return View();
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
}
