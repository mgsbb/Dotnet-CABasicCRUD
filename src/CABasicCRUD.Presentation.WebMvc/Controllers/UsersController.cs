using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Users;
using CABasicCRUD.Application.Features.Users.GetUserById;
using CABasicCRUD.Application.Features.Users.SearchUsers;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
using CABasicCRUD.Presentation.WebMvc.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABasicCRUD.Presentation.WebMvc.Controllers;

[Authorize]
[Route("users")]
public sealed class UsersController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUser _currentUser;

    public UsersController(IMediator mediator, ICurrentUser currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(UserListViewModel model)
    {
        // if (!ModelState.IsValid)
        //     return RedirectToAction(nameof(Index));

        SearchUsersQuery query = new(
            model.SearchTerm,
            model.Page,
            model.PageSize,
            model.OrderBy,
            model.SortDirection
        );

        Result<IReadOnlyList<UserResult>> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return View(new List<UserListItemViewModel>());
        }

        IReadOnlyList<UserListItemViewModel> userListItems = result
            .Value.Select(u => new UserListItemViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
            })
            .ToList();

        UserListViewModel viewModel = new() { Users = userListItems };

        return View(viewModel);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        GetUserByIdQuery query = new((UserId)id);
        Result<UserResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
        {
            return NotFound();
        }

        UserDetailsViewModel viewModel = new()
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Email = result.Value.Email,
        };

        return View(viewModel);
    }
}
