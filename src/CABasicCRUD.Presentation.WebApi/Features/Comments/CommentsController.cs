using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Application.Features.Comments.DeleteComment;
using CABasicCRUD.Application.Features.Comments.GetCommentById;
using CABasicCRUD.Application.Features.Comments.UpdateComment;
using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Comments.Contracts;
using CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebApi.Features.Comments;

[ApiController]
[Route("/api/v1/[controller]")]
public class CommentsController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(CommentResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CommentResponse?>> GetCommentById(Guid id)
    {
        GetCommentByIdQuery query = new((CommentId)id);

        Result<CommentResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value == null)
        {
            return HandleResultFailure(result);
        }

        CommentResponse commentResponse = result.Value.ToCommentResponse();

        return Ok(commentResponse);
    }

    [Authorize]
    [HttpPatch("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest request, Guid id)
    {
        UpdateCommentCommand command = new((CommentId)id, request.Body);

        Result result = await _mediator.Send(request: command);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        DeleteCommentCommand command = new((CommentId)id);

        Result result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return NoContent();
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
        if (
            result.Error == Application.Features.Users.UserErrors.NotFound
            || result.Error == Application.Features.Posts.PostErrors.NotFound
            || result.Error == Application.Features.Comments.CommentErrors.NotFound
        )
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }
        if (result.Error == AuthErrors.Forbidden)
        {
            return HandleProblem(StatusCodes.Status403Forbidden, detail: result.Error.Message);
        }
        if (result.Error == AuthErrors.Unauthenticated)
        {
            return HandleProblem(StatusCodes.Status401Unauthorized, detail: result.Error.Message);
        }

        return HandleProblem(StatusCodes.Status500InternalServerError);
    }
}
