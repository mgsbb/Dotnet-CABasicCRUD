using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Identity.Auth.Commands.LoginUser;
using CABasicCRUD.Application.Features.Identity.Auth.Commands.RegisterUser;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Presentation.WebMvc.Common;
using CABasicCRUD.Presentation.WebMvc.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;
    private readonly IConfiguration _configuration;

    public AuthController(
        IMediator mediator,
        ICurrentUser currentUser,
        IConfiguration configuration
    )
    {
        _mediator = mediator;
        _currentUser = currentUser;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (_currentUser.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        RegisterUserCommand command = new(model.Name, model.Username, model.Email, model.Password);

        Result<AuthResult> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error == null || result.IsSuccess)
                throw new InvalidOperationException();

            if (result.Error == AuthErrors.AlreadyExistsEmail)
            {
                ModelState.AddModelError(nameof(model.Email), result.Error.Message.ToString());
            }
            if (result.Error == AuthErrors.AlreadyExistsUsername)
            {
                ModelState.AddModelError(nameof(model.Username), result.Error.Message.ToString());
            }
            // TODO: handle this better
            if (result is IValidationResult validationResult)
            {
                string passwordErrorMessage = "";

                foreach (var e in validationResult.Errors)
                {
                    if (e.Code == "Password")
                        passwordErrorMessage += $" {e.Message}";
                }
                ModelState.AddModelError(nameof(model.Password), passwordErrorMessage);
            }

            return View(model);
        }

        Response.Cookies.Append(
            "access_token",
            result.Value.Token,
            CookieOptionsFactory.CreateAccessTokenCookieOptions(_configuration)
        );

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (_currentUser.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        LoginUserCommand command = new(model.Email, model.Password);

        Result<AuthResult> result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error == null || result.IsSuccess)
                throw new InvalidOperationException();

            if (result.Error == AuthErrors.InvalidCredentials)
            {
                ModelState.AddModelError(string.Empty, result.Error.Message.ToString());
            }

            return View(model);
        }

        Response.Cookies.Append(
            "access_token",
            result.Value.Token,
            CookieOptionsFactory.CreateAccessTokenCookieOptions(_configuration)
        );

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete(
            "access_token",
            CookieOptionsFactory.CreateAccessTokenCookieOptions(_configuration)
        );

        return RedirectToAction("Login", "Auth");
    }
}
