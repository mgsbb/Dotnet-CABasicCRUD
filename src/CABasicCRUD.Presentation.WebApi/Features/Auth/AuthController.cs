using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Auth.LoginUser;
using CABasicCRUD.Application.Features.Auth.RegisterUser;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Presentation.WebApi.Common.Abstractions;
using CABasicCRUD.Presentation.WebApi.Features.Auth.Contracts;
using CABasicCRUD.Presentation.WebApi.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebApi.Features.Auth;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class AuthController(IMediator mediator) : ApiController
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

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> LoginUser([FromBody] LoginUserRequest loginUserRequest)
    {
        LoginUserCommand command = new(loginUserRequest.Email, loginUserRequest.Password);
        Result<AuthResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            return HandleResultFailure(result);
        }

        Response.Cookies.Append("access_token", result.Value.Token);

        AuthResponse authResponse = result.Value.ToAuthResponse();

        return Ok(authResponse);
    }

    private ObjectResult HandleResultFailure(Result result)
    {
        // redundant check?
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
        if (result.Error == AuthErrors.AlreadyExists)
        {
            return HandleProblem(StatusCodes.Status409Conflict, detail: result.Error.Message);
        }
        if (result.Error == AuthErrors.InvalidCredentials)
        {
            return HandleProblem(StatusCodes.Status401Unauthorized, detail: result.Error.Message);
        }
        return HandleProblem(StatusCodes.Status500InternalServerError);
    }
}
