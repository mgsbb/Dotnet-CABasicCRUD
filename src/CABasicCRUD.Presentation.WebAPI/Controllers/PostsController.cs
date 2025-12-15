using CABasicCRUD.Application.Posts.Commands.CreatePost;
using CABasicCRUD.Application.Posts.Commands.DeletePost;
using CABasicCRUD.Application.Posts.Commands.UpdatePost;
using CABasicCRUD.Application.Posts.DTOs;
using CABasicCRUD.Application.Posts.Queries.GetAllposts;
using CABasicCRUD.Application.Posts.Queries.GetPostById;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Presentation.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class PostsController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(type: typeof(PostDTO), statusCode: StatusCodes.Status201Created)]
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] CreatePostDTO createPostDTO)
    {
        CreatePostCommand command = new(CreatePostDTO: createPostDTO);

        Result<PostDTO> postDTOResult = await _mediator.Send(request: command);

        if (postDTOResult.IsFailure || postDTOResult.Value is null)
        {
            return HandleBadRequest(postDTOResult);
        }

        return CreatedAtAction(
            actionName: nameof(GetPostById),
            routeValues: new { id = postDTOResult.Value.PostId.Value },
            value: postDTOResult.Value
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(PostDTO), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PostDTO?>> GetPostById(Guid id)
    {
        GetPostByIdQuery query = new(PostId: (PostId)id);

        Result<PostDTO> postDTOResult = await _mediator.Send(request: query);

        if (postDTOResult.IsFailure || postDTOResult.Value == null)
        {
            return NotFound();
        }

        return Ok(postDTOResult.Value);
    }

    [HttpGet]
    [ProducesResponseType(
        type: typeof(IReadOnlyList<PostDTO>),
        statusCode: StatusCodes.Status200OK
    )]
    public async Task<ActionResult<IReadOnlyList<PostDTO>>> GetAllPosts()
    {
        GetAllPostsQuery query = new();

        Result<IReadOnlyList<PostDTO>> postDTOListResult = await _mediator.Send(request: query);

        if (postDTOListResult.IsFailure || postDTOListResult.Value == null)
            return Ok();

        return Ok(postDTOListResult.Value);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDTO updatePostDTO, Guid id)
    {
        UpdatePostCommand command = new(UpdatePostDTO: updatePostDTO, PostId: (PostId)id);

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
