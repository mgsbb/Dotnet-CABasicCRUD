using CABasicCRUD.Application.Features.Users;
using CABasicCRUD.Application.Features.Users.GetUserById;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebAPI.Common.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Features.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Features.Users;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class UsersController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(type: typeof(UserResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse?>> GetUserById(Guid id)
    {
        GetUserByIdQuery query = new((UserId)id);

        Result<UserResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value == null)
        {
            return HandleProblem(
                statusCode: StatusCodes.Status404NotFound,
                detail: "User Not Found"
            );
        }

        UserResponse userResponse = result.Value.ToUserResponse();

        return Ok(userResponse);
    }
}
