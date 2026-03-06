using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Commands;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Application.Features.Identity.Users.Queries.GetUserById;
using CABasicCRUD.Application.Features.Identity.Users.Queries.SearchUsers;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Queries.SearchPosts;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Presentation.WebMvc.Models.Posts;
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

        if (result.IsFailure)
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
    public async Task<IActionResult> Details(Guid id, UserDetailsViewModel model)
    {
        ViewBag.Error = TempData["Error"];

        GetUserByIdQuery query = new((UserId)id);
        Result<UserResult> result = await _mediator.Send(query);

        if (result.IsFailure)
        {
            return NotFound();
        }

        SearchPostsQuery searchPostsQuery = new(
            model.PostsList.SearchTerm,
            model.PostsList.Page,
            model.PostsList.PageSize,
            model.PostsList.OrderBy,
            model.PostsList.SortDirection,
            (UserId)id
        );

        Result<IReadOnlyList<PostWithAuthorResult>> postsResult = await _mediator.Send(
            searchPostsQuery
        );

        if (postsResult.IsFailure || postsResult.Value is null)
        {
            return View();
        }

        IReadOnlyList<PostListItemViewModel> postListItems = postsResult
            .Value.Select(p => new PostListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ContentPreview =
                    p.Content.Length <= 100
                        ? p.Content
                        : string.Concat(p.Content.AsSpan(0, 100), "..."),
                UserId = p.UserId,
                AuthorName = p.AuthorName,
            })
            .ToList();

        PostListViewModel postListViewModel = new() { Posts = postListItems };

        UserDetailsViewModel viewModel = new()
        {
            Id = result.Value.Id,
            Name = result.Value.Name,
            Username = result.Value.Username,
            PostsList = postListViewModel,
            Bio = result.Value.Bio,
            ProfileImageUrl = result.Value.ProfileImageUrl,
        };

        return View(viewModel);
    }

    [HttpGet("{id}/edit")]
    public IActionResult Edit() => View();

    [HttpPost("{id}/edit/profile")]
    public async Task<IActionResult> EditUserProfile(UserEditViewModel model, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit");
        }

        UpdateUserProfileCommand command = new((UserId)id, model.FullName, model.Bio);
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error is null)
                throw new InvalidOperationException();

            if (result.Error == AuthErrors.Forbidden)
            {
                ModelState.AddModelError(string.Empty, "Cannot edit another user");
            }
            if (result is IValidationResult validationResult)
            {
                string errorMessage = "";
                foreach (var e in validationResult.Errors)
                {
                    errorMessage += e.Message + " ";
                }
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            return View("Edit");
        }

        TempData["SuccessMessage"] = "User profile updated successfully!";

        return View("Edit");
        // return RedirectToAction(nameof(Details), new { id = result.Value.Id.Value });
    }

    [HttpPost("{id}/edit/email")]
    public async Task<IActionResult> EditUserEmail(UserEditViewModel model, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Email is null)
        {
            ModelState.AddModelError(string.Empty, "Please enter a valid email.");
            return View("Edit");
        }

        UpdateUserEmailCommand command = new((UserId)id, model.Email);
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error is null)
                throw new InvalidOperationException();

            if (result.Error == AuthErrors.Forbidden)
            {
                ModelState.AddModelError(string.Empty, "Cannot edit another user");
            }
            if (result.Error == AuthErrors.AlreadyExistsEmail)
            {
                ModelState.AddModelError(string.Empty, "Email already in use.");
            }

            return View("Edit");
        }

        TempData["SuccessMessage"] = "User email updated successfully!";

        return View("Edit");
        // return RedirectToAction(nameof(Details), new { id = result.Value.Id.Value });
    }
}
