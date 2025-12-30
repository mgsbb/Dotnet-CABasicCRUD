using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Application.Features.Comments.CreateComment;
using CABasicCRUD.Application.Features.Comments.GetAllCommentsOfPost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Presentation.WebAPI.Common.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Features.Comments.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Features.Comments;

[ApiController]
[Route("/api/v1/posts/{postId:guid}/comments")]
public class PostCommentsController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(type: typeof(CommentResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CommentResponse>> CreateComment(
        [FromBody] CreateCommentRequest request,
        Guid postId
    )
    {
        CreateCommentCommand command = new(request.Body, (PostId)postId);

        Result<CommentResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
        }

        CommentResponse commentResponse = result.Value.ToCommentResponse();

        return CreatedAtAction(
            actionName: nameof(CommentsController.GetCommentById),
            controllerName: "Comments",
            routeValues: new { id = commentResponse.Id },
            value: commentResponse
        );
    }

    [HttpGet]
    [ProducesResponseType(
        type: typeof(IReadOnlyList<CommentResponse>),
        statusCode: StatusCodes.Status200OK
    )]
    public async Task<ActionResult<IReadOnlyList<CommentResponse>>> GetAllCommentsOfPost(
        Guid postId
    )
    {
        GetAllCommentsOfPostQuery query = new((PostId)postId);

        Result<IReadOnlyList<CommentResult>> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value == null)
            return Ok();

        IReadOnlyList<CommentResponse> commentResponses = result.Value.ToListCommentResponse();

        return Ok(commentResponses);
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
