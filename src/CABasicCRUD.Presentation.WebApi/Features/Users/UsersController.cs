using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Identity.Auth.Commands;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Commands;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Application.Features.Identity.Users.Queries.GetUserById;
using CABasicCRUD.Application.Features.Identity.Users.Queries.SearchUsers;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Users.Contracts;
using CABasicCRUD.Presentation.WebApi.RateLimiter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CABasicCRUD.Presentation.WebApi.Features.Users;

// ========================================================================================================================
// ========================================================================================================================

[EnableRateLimiting(RateLimitPolicies.Authenticated)]
[ApiController]
[Route("/api/v1/[controller]")]
public sealed class UsersController(IMediator mediator) : ApiController
{
    private readonly IMediator _mediator = mediator;

    // ========================================================================================================================

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

    // ========================================================================================================================

    [HttpGet]
    [ProducesResponseType(
        type: typeof(IReadOnlyList<UserResponse>),
        statusCode: StatusCodes.Status200OK
    )]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> SearchUsersAsync(
        [FromQuery] SearchUsersRequestQueryParams queryParams
    )
    {
        SearchUsersQuery query = new(
            queryParams.SearchTerm,
            queryParams.Page,
            queryParams.PageSize,
            queryParams.UserOrderBy,
            queryParams.SortDirection
        );

        Result<IReadOnlyList<UserResult>> result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        IReadOnlyList<UserResponse> userResponses = result.Value.ToUserResponse();

        return Ok(userResponses);
    }

    // ========================================================================================================================

    [Authorize]
    [HttpPatch("{id}/email")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserEmail(
        [FromBody] UpdateUserEmailRequest request,
        Guid id
    )
    {
        UpdateUserEmailCommand command = new((UserId)id, request.Email);

        Result result = await _mediator.Send(request: command);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return NoContent();
    }

    // ========================================================================================================================

    [Authorize]
    [HttpPatch("{id}/profile")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateUserProfile(
        [FromBody] UpdateUserProfileRequest request,
        Guid id
    )
    {
        UpdateUserProfileCommand command = new((UserId)id, request.FullName, request.Bio);

        Result result = await _mediator.Send(request: command);

        if (result.IsFailure)
        {
            return HandleResultFailure(result);
        }

        return NoContent();
    }

    // ========================================================================================================================

    [Authorize]
    [HttpPatch("{id}/password")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserPassword(
        [FromBody] UpdateUserPasswordRequest request,
        Guid id
    )
    {
        UpdateUserPasswordCommand command = new(
            (UserId)id,
            request.OldPassword,
            request.NewPassword,
            request.NewPasswordConfirmed
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
    [HttpPatch("{id}/profile-image")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserProfileImage(Guid id, IFormFile formFile)
    {
        UpdateUserProfileImageCommand command = new(
            (UserId)id,
            formFile.OpenReadStream(),
            formFile.FileName,
            formFile.ContentType
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
    [HttpPatch("{id}/cover-image")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserCoverImage(Guid id, IFormFile formFile)
    {
        UpdateUserCoverImageCommand command = new(
            (UserId)id,
            formFile.OpenReadStream(),
            formFile.FileName,
            formFile.ContentType
        );

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
        if (result.Error == Application.Features.Identity.Users.Common.UserErrors.NotFound)
        {
            return HandleProblem(StatusCodes.Status404NotFound, detail: result.Error.Message);
        }

        if (result.Error == AuthErrors.AlreadyExistsEmail)
        {
            return HandleProblem(StatusCodes.Status409Conflict, detail: result.Error.Message);
        }
        if (
            result.Error == AuthErrors.PasswordsMismatch
            || result.Error == AuthErrors.InvalidPassword
            || result.Error == AuthErrors.Forbidden
        )
        {
            return HandleProblem(StatusCodes.Status403Forbidden, detail: result.Error.Message);
        }
        return HandleProblem(StatusCodes.Status500InternalServerError);
    }
}

// ========================================================================================================================
// ========================================================================================================================
