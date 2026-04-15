using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Commands.CreatePost;
using CABasicCRUD.Application.Features.Posts.Posts.Commands.DeletePost;
using CABasicCRUD.Application.Features.Posts.Posts.Commands.UpdatePost;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Queries.GetPostByIdWithAuthor;
using CABasicCRUD.Application.Features.Posts.Posts.Queries.SearchPosts;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;
using CABasicCRUD.Domain.Posts.Posts;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CABasicCRUD.Presentation.WebApi.Features.Posts;

// ========================================================================================================================
// ========================================================================================================================

[EnableRateLimiting(RateLimitPolicies.Authenticated)]
[ApiController]
[Route("/api/v1/[controller]")]
public class PostsController(IMediator mediator, ICurrentUser currentUser) : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly ICurrentUser _currentUser = currentUser;

    // ========================================================================================================================

    [Authorize]
    [HttpPost]
    [RequestSizeLimit(100_000_000)]
    [ProducesResponseType(type: typeof(PostResponse), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PostResponse>> CreatePost([FromForm] CreatePostRequest request)
    {
        if (request.Files.Count > 5)
        {
            return HandleProblem(
                StatusCodes.Status400BadRequest,
                "Cannot upload more than 5 media items."
            );
        }

        int videoCount = 0;

        foreach (var file in request.Files)
        {
            if (file.ContentType.StartsWith("video/"))
            {
                videoCount++;
                continue;
            }

            if (file.ContentType.StartsWith("image/"))
                continue;

            return HandleProblem(
                StatusCodes.Status400BadRequest,
                "Can only upload video or image."
            );
        }

        if (videoCount > 1)
        {
            return HandleProblem(
                StatusCodes.Status400BadRequest,
                "Cannot upload more than 1 video."
            );
        }

        var media = request
            .Files.Select(f => new CreatePostMedia(
                f.OpenReadStream(),
                f.FileName,
                f.ContentType.StartsWith("video/") ? MediaType.Video : MediaType.Image,
                f.ContentType
            ))
            .ToList();

        CreatePostCommand command = new(
            request.Title,
            request.Content,
            (UserId)_currentUser.UserId,
            media
        );

        Result<PostResult> result = await _mediator.Send(request: command);

        if (result.IsFailure)
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

    // ========================================================================================================================

    [HttpGet("{id}")]
    [ProducesResponseType(
        type: typeof(PostWithAuthorResponse),
        statusCode: StatusCodes.Status200OK
    )]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostWithAuthorResponse?>> GetPostById(Guid id)
    {
        GetPostByIdWithAuthorQuery query = new(PostId: (PostId)id);

        Result<PostWithAuthorResult> result = await _mediator.Send(request: query);

        if (result.IsFailure || result.Value == null)
        {
            return HandleResultFailure(result);
        }

        PostWithAuthorResponse postResponse = result.Value.ToPostWithAuthorResponse();

        return Ok(postResponse);
    }

    // ========================================================================================================================

    [HttpGet]
    [ProducesResponseType(
        type: typeof(IReadOnlyList<PostWithAuthorResponse>),
        statusCode: StatusCodes.Status200OK
    )]
    public async Task<ActionResult<IReadOnlyList<PostWithAuthorResponse>>> SearchPosts(
        [FromQuery] SearchPostsRequestQueryParams queryParams
    )
    {
        SearchPostsQuery query = new(
            queryParams.SearchTerm,
            queryParams.Page,
            queryParams.PageSize,
            queryParams.PostOrderBy,
            queryParams.SortDirection,
            (UserId)queryParams.UserId! ?? null
        );

        Result<IReadOnlyList<PostWithAuthorResult>> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value == null)
            return Ok();

        IReadOnlyList<PostWithAuthorResponse> postResponses =
            result.Value.ToListPostWithAuthorResponse();

        return Ok(postResponses);
    }

    // ========================================================================================================================

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

    // ========================================================================================================================

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
        if (result.Error == Application.Features.Posts.Posts.Common.PostErrors.NotFound)
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

// ========================================================================================================================
// ========================================================================================================================
