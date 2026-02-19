using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Application.Features.Comments.DeleteComment;
using CABasicCRUD.Application.Features.Comments.GetCommentById;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebMvc.Models.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

[Authorize]
[Route("comments")]
public sealed class CommentsController(IMediator mediator, ICurrentUser currentUser) : Controller
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUser _currentUser = currentUser;

    [HttpGet("{id}/delete")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        GetCommentByIdQuery query = new((CommentId)id);
        Result<CommentResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }

        var viewModel = new CommentDeleteViewModel
        {
            Id = result.Value.Id,
            Body = result.Value.Body,
            PostId = result.Value.PostId,
        };

        return View(viewModel);
    }

    [HttpPost("{id}/delete")]
    public async Task<IActionResult> DeleteAsync(CommentDeleteViewModel model, Guid id)
    {
        // get postId from hidden input from get DeleteAsync page, possible risk of tampering

        // GetCommentByIdQuery query = new((CommentId)id);
        // Result<CommentResult> commentResult = await _mediator.Send(query);

        // if (commentResult.IsFailure || commentResult.Value is null)
        // {
        //     return NotFound();
        // }

        DeleteCommentCommand command = new((CommentId)id, (UserId)_currentUser.UserId);
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error is null)
                throw new InvalidOperationException();

            if (result.Error == AuthErrors.Forbidden)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete comment of another user");
            }

            return View(model);
        }

        return RedirectToAction(
            controllerName: "Posts",
            actionName: "Details",
            routeValues: new { id = model.PostId }
        );
    }
}
