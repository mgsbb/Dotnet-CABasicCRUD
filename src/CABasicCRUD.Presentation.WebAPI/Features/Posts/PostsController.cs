using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using CABasicCRUD.Application.Features.Posts.DeletePost;
using CABasicCRUD.Application.Features.Posts.GetAllposts;
using CABasicCRUD.Application.Features.Posts.GetPostById;
using CABasicCRUD.Application.Features.Posts.UpdatePost;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Presentation.WebAPI.Common.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Features.Posts.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Features.Posts;

[ApiController]
[Route("/api/v1/[controller]")]
public class PostsController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(type: typeof(PostResponse), statusCode: StatusCodes.Status201Created)]
    public async Task<ActionResult<PostResponse>> CreatePost([FromBody] CreatePostRequest request)
    {
        CreatePostCommand command = new(request.Title, request.Content);

        Result<PostResult> result = await _mediator.Send(request: command);

        if (result.IsFailure || result.Value is null)
        {
            return HandleBadRequest(result);
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
            return NotFound();
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

        IReadOnlyList<PostResponse> postResponses = result.Value.ToListPostResult();

        return Ok(postResponses);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request, Guid id)
    {
        UpdatePostCommand command = new(PostId: (PostId)id, request.Title, request.Content);

        Result result = await _mediator.Send(request: command);

        if (result.IsFailure)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        DeletePostCommand command = new(PostId: (PostId)id);

        await _mediator.Send(request: command);

        return NoContent();
    }
}
