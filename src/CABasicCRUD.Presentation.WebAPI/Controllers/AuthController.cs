using CABasicCRUD.Application.Auth.Commands.LoginUser;
using CABasicCRUD.Application.Auth.Commands.RegisterUser;
using CABasicCRUD.Application.Auth.DTOs;
using CABasicCRUD.Application.Auth.Errors;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Presentation.WebAPI.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Contracts.Auth;
using CABasicCRUD.Presentation.WebAPI.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class AuthController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        RegisterUserCommand command = new(request.Name, request.Email, request.Password);
        Result<UserResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            return HandleBadRequest(result);
        }

        return CreatedAtAction(
            actionName: nameof(GetUserById),
            routeValues: new { id = result.Value.Id },
            value: result.Value
        );
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest)
    {
        LoginUserCommand command = new(loginUserRequest.Email, loginUserRequest.Password);
        Result<LoginUserResult> result = await _mediator.Send(command);

        if (result.IsFailure && result.Error == AuthErrors.InvalidCredentials)
        {
            return Unauthorized();
        }
        if (result is IValidationResult || result.Value is null)
        {
            return HandleBadRequest(result);
        }

        Response.Cookies.Append("access_token", result.Value.Token);

        LoginUserResponse loginUserResponse = new(
            result.Value.Id,
            result.Value.Name,
            result.Value.Email
        );

        return Ok(loginUserResponse);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserById(Guid id)
    {
        return Ok();
    }
}
