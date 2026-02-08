using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using CABasicCRUD.Application.Features.Posts.DeletePost;
using CABasicCRUD.Application.Features.Posts.GetAllposts;
using CABasicCRUD.Application.Features.Posts.GetPostById;
using CABasicCRUD.Application.Features.Posts.UpdatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CABasicCRUD.Presentation.WebApi.Features.Posts;

[EnableRateLimiting(RateLimitPolicies.Authenticated)]
[ApiController]
[Route("/api/v1/[controller]")]
public class PostsController(IMediator mediator, ICurrentUser currentUser) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUser _currentUser = currentUser;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(type: typeof(PostResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PostResponse>> CreatePost([FromBody] CreatePostRequest request)
    {
        CreatePostCommand command = new(
            request.Title,
            request.Content,
            (UserId)_currentUser.UserId
        );

        Result<PostResult> result = await _mediator.Send(request: command);

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
        }

        PostResponse postResponse = result.Value.ToPostResponse();

        return CreatedAtAction(
            actionName: nameof(GetPostById),
            routeValues: new { id = postResponse.Id },
            value: postResponse
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(PostResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostResponse?>> GetPostById(Guid id)
    {
        GetPostByIdQuery query = new(PostId: (PostId)id);

        Result<PostResult> result = await _mediator.Send(request: query);

        if (result.IsFailure || result.Value == null)
        {
            return HandleResultFailure(result);
        }

        PostResponse postResponse = result.Value.ToPostResponse();

        return Ok(postResponse);
    }

    [HttpGet]
    [ProducesResponseType(
        type: typeof(IReadOnlyList<PostResponse>),
        statusCode: StatusCodes.Status200OK
    )]
    public async Task<ActionResult<IReadOnlyList<PostResponse>>> GetAllPosts()
    {
        GetAllPostsQuery query = new();

        Result<IReadOnlyList<PostResult>> result = await _mediator.Send(request: query);

        if (result.IsFailure || result.Value == null)
            return Ok();

        IReadOnlyList<PostResponse> postResponses = result.Value.ToListPostResponse();

        return Ok(postResponses);
    }

    [Authorize]
    [HttpPatch("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request, Guid id)
    {
        UpdatePostCommand command = new(
            PostId: (PostId)id,
            request.Title,
            request.Content,
            (UserId)_currentUser.UserId
        );

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
    public async Task<IActionResult> DeletePost(Guid id)
    {
        DeletePostCommand command = new((PostId)id, (UserId)_currentUser.UserId);

        Result result = await _mediator.Send(request: command);

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
        if (result.Error == Application.Features.Posts.PostErrors.NotFound)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }
        if (result.Error == AuthErrors.Forbidden)
        {
            return HandleProblem(StatusCodes.Status403Forbidden, detail: result.Error.Message);
        }
        return HandleProblem(StatusCodes.Status500InternalServerError);
    }
}
