using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Users;
using CABasicCRUD.Application.Features.Users.DeleteUser;
using CABasicCRUD.Application.Features.Users.GetUserById;
using CABasicCRUD.Application.Features.Users.UpdateUser;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Common.Security.Authorization;
using CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebApi.Features.Users;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class UsersController(IMediator mediator) : ApiController
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
            return HandleResultFailure(result);
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

        if (updateResult.IsFailure)
        {
            return HandleResultFailure(updateResult);
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

        if (deleteResult.IsFailure)
        {
            return HandleResultFailure(deleteResult);
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
        if (result.Error == Application.Features.Users.UserErrors.NotFound)
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
