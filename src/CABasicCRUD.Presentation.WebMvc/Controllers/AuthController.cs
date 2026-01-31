using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Auth.LoginUser;
using CABasicCRUD.Application.Features.Auth.RegisterUser;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Presentation.WebMvc.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public AuthController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
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

        RegisterUserCommand command = new(model.Name, model.Email, model.Password);

        Result<AuthResult> result = await _mediator.Send(command);

        if (result.IsFailure || result.Value is null)
        {
            if (result.Error == null)
                throw new InvalidOperationException();

            ModelState.AddModelError(string.Empty, result.Error.ToString());
            return View(model);
        }

        Response.Cookies.Append("access_token", result.Value.Token);

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

        if (result.IsFailure || result.Value is null)
        {
            if (result.Error == null)
                throw new InvalidOperationException();

            ModelState.AddModelError(string.Empty, result.Error.ToString());
            return View(model);
        }

        Response.Cookies.Append("access_token", result.Value.Token);

        return RedirectToAction("Index", "Home");
    }
}
