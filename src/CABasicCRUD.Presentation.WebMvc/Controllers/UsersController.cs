using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.SearchPosts;
using CABasicCRUD.Application.Features.Users;
using CABasicCRUD.Application.Features.Users.GetUserById;
using CABasicCRUD.Application.Features.Users.SearchUsers;
using CABasicCRUD.Application.Features.Users.UpdateUser;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;
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
    public async Task<IActionResult> Details(Guid id, UserDetailsViewModel model)
    {
        GetUserByIdQuery query = new((UserId)id);
        Result<UserResult> result = await _mediator.Send(query);

        if (result.IsFailure || result.Value is null)
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
        };

        return View(viewModel);
    }

    [HttpGet("{id}/edit")]
    public IActionResult Edit() => View();

    [HttpPost("{id}/edit")]
    public async Task<IActionResult> Edit(UserEditViewModel model, Guid id)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        UpdateUserCommand command = new((UserId)id, model.Name, model.Email);
        Result result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            if (result.Error is null)
                throw new InvalidOperationException();

            if (result.Error == AuthErrors.Forbidden)
            {
                ModelState.AddModelError(string.Empty, "Cannot edit another user");
            }

            return View(model);
        }

        return RedirectToAction("Index");
        // return RedirectToAction(nameof(Details), new { id = result.Value.Id.Value });
    }
}
