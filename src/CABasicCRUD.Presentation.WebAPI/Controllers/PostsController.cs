using CABasicCRUD.Application.Posts.Commands.CreatePost;
using CABasicCRUD.Application.Posts.Commands.DeletePost;
using CABasicCRUD.Application.Posts.Commands.UpdatePost;
using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Queries.GetAllposts;
using CABasicCRUD.Application.Posts.Queries.GetPostById;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Presentation.WebAPI.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Contracts.Posts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Controllers;

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

        return CreatedAtAction(
            actionName: nameof(GetPostById),
            routeValues: new { id = result.Value.Id },
            value: result.Value
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

        return Ok(result.Value);
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

        return Ok(result.Value);
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
