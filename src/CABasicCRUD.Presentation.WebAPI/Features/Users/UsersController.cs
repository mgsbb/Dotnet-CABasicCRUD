using CABasicCRUD.Application.Features.Users;
using CABasicCRUD.Application.Features.Users.DeleteUser;
using CABasicCRUD.Application.Features.Users.GetUserById;
using CABasicCRUD.Application.Features.Users.UpdateUser;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebAPI.Common.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Common.Security.Authorization;
using CABasicCRUD.Presentation.WebAPI.Features.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateUser(
        Guid id,
        [FromBody] UpdateUserRequest request,
        [FromServices] IAuthorizationService authorizationService
    )
    {
        GetUserByIdQuery query = new((UserId)id);

        Result<UserResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: "User not found.");
        }

        var authResult = await authorizationService.AuthorizeAsync(
            HttpContext.User,
            id,
            AuthorizationPolicies.ResourceOwner
        );

        if (!authResult.Succeeded)
        {
            return HandleProblem(
                StatusCodes.Status403Forbidden,
                detail: "You don't have permission to perform the requested action."
            );
        }

        UpdateUserCommand command = new((UserId)id, request.Name, request.Email);

        Result updateResult = await _mediator.Send(command);

        // TODO: Send 400 before 404 and 403.
        // Redundant check: updateResult is IValidationResult
        if (updateResult.IsFailure && updateResult is IValidationResult)
        {
            return HandleBadRequest(updateResult);
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUser(
        Guid id,
        [FromServices] IAuthorizationService authorizationService
    )
    {
        GetUserByIdQuery query = new((UserId)id);

        Result<UserResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: "User not found.");
        }

        var authResult = await authorizationService.AuthorizeAsync(
            HttpContext.User,
            id,
            AuthorizationPolicies.ResourceOwner
        );

        if (!authResult.Succeeded)
        {
            return HandleProblem(
                StatusCodes.Status403Forbidden,
                detail: "You don't have permission to perform the requested action."
            );
        }

        DeleteUserCommand command = new((UserId)id);

        Result deleteResult = await _mediator.Send(command);

        // TODO: Send 400 before 404 and 403.

        if (deleteResult.IsFailure)
        {
            return HandleProblem(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}
