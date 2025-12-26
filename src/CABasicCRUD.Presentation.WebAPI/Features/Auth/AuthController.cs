using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Auth.LoginUser;
using CABasicCRUD.Application.Features.Auth.RegisterUser;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Presentation.WebAPI.Common.Abstractions;
using CABasicCRUD.Presentation.WebAPI.Features.Auth.Contracts;
using CABasicCRUD.Presentation.WebAPI.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebAPI.Features.Auth;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class AuthController(IMediator mediator) : APIController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        RegisterUserCommand command = new(request.Name, request.Email, request.Password);
        Result<AuthResult> result = await _mediator.Send(command);

        if (result.IsFailure && result is IValidationResult)
        {
            return HandleBadRequest(result);
        }
        if (result.IsFailure && result.Error == AuthErrors.AlreadyExists)
        {
            return HandleProblem(StatusCodes.Status409Conflict, detail: "Email already in use.");
        }
        if (result.IsFailure || result.Value is null)
        {
            return HandleProblem(StatusCodes.Status500InternalServerError);
        }

        Response.Cookies.Append("access_token", result.Value.Token);

        AuthResponse authResponse = result.Value.ToAuthResponse();

        return CreatedAtAction(
            actionName: nameof(UsersController.GetUserById),
            controllerName: "Users",
            routeValues: new { id = authResponse.Id },
            value: authResponse
        );
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest)
    {
        LoginUserCommand command = new(loginUserRequest.Email, loginUserRequest.Password);
        Result<AuthResult> result = await _mediator.Send(command);

        if (result.IsFailure && result.Error == AuthErrors.InvalidCredentials)
        {
            return Unauthorized();
        }
        if (result is IValidationResult || result.Value is null)
        {
            return HandleBadRequest(result);
        }

        Response.Cookies.Append("access_token", result.Value.Token);

        AuthResponse authResponse = result.Value.ToAuthResponse();

        return Ok(authResponse);
    }
}
